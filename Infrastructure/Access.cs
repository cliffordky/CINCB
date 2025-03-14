using Ardalis.Result;
using Core.Models.Data;
using Core.Repository;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    public class Access : Core.IAccess
    {
        private readonly ILogger<Access> _logger;
        private readonly IUser _userRepository;
        private readonly IAspNetUserOrganization _aspNetUserOrganizationRepository;

        public Access(ILogger<Infrastructure.Access> logger, Core.Repository.IUser userRepository, Core.Repository.IAspNetUserOrganization aspNetUserOrganization)
        {
            _logger = logger;
            _userRepository = userRepository;
            _aspNetUserOrganizationRepository = aspNetUserOrganization;
        }

        public async Task<Result<User>> GetUserByIdAsync(int Id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(Id);
                if (user == null)
                    return Result<User>.NotFound();

                return Result<User>.Success(user);
            }
            catch (Exception Ex)
            {
                return Result<User>.Error(Ex.Message);
            }
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var result = await _userRepository.GetAllAsync();

            return result.ToList();
        }

        public async Task<Result<User>> GetUserByPublicKeyAsync(Guid publicKey)
        {
            try
            {
                var user = await _userRepository.GetUserByPublicKeyAsync(publicKey);
                if (user == null)
                    return Result<User>.NotFound();

                return Result<User>.Success(user);
            }
            catch (Exception Ex)
            {
                return Result<User>.Error(Ex.Message);
            }
        }

        public async Task<Result<User>> UpdateUserAsync(Core.Models.Data.User entity)
        {
            try
            {
                var user = await GetUserByIdAsync(entity.Id);
                if (!user.IsSuccess)
                    return Result<User>.NotFound();

                user.Value.FirstName = entity.FirstName;
                user.Value.LastName = entity.LastName;
                user.Value.PhoneNumber = entity.PhoneNumber;
                user.Value.ImageSlug = entity.ImageSlug;

                var updateResult = await _userRepository.UpdateAsync(user.Value);
                if (updateResult == null)
                    return Result<User>.Error("Failed to update user");

                return Result<User>.Success(updateResult);
            }
            catch (Exception Ex)
            {
                return Result<User>.Error(Ex.Message);
            }
        }

        public async Task<List<AspNetUserOrganization>> GetUserOrganizationsByUserIdAsync(int UserId)
        {
            var results = await _aspNetUserOrganizationRepository.GetAllByUserIdAsync(UserId);
            return results.ToList();
        }

        public async Task<Result<bool>> UpsertUserOrganizationsAsync(int UserId, List<int> OrganizationIds)
        {
            try
            {
                var existingIds = await _aspNetUserOrganizationRepository.GetAllByUserIdAsync(UserId);

                var addedOrganizationIds = OrganizationIds.Except(existingIds.Select(x => x.OrganizationId)).ToList();
                foreach (var orgId in addedOrganizationIds)
                {
                    await _aspNetUserOrganizationRepository.AddAsync(new AspNetUserOrganization
                    {
                        AspNetUserId = UserId,
                        OrganizationId = orgId,
                    });
                }

                var removedOrganizationIds = existingIds.Select(x => x.OrganizationId).Except(OrganizationIds).ToList();
                foreach (var orgId in removedOrganizationIds)
                {
                    var org = existingIds.FirstOrDefault(x => x.OrganizationId == orgId);
                    if (org != null)
                    {
                        await _aspNetUserOrganizationRepository.DeleteAsync(org.Id);
                    }
                }

                return Result<bool>.Success(true);
            }
            catch
            {
                return Result<bool>.Success(false);
            }
        }
    }
}