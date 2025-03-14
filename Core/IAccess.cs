using Ardalis.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IAccess
    {
        Task<List<Models.Data.User>> GetUsersAsync();
        Task<Result<Models.Data.User>> GetUserByIdAsync(int Id);
        Task<Result<Models.Data.User>> GetUserByPublicKeyAsync(Guid publicKey);
        Task<Result<Core.Models.Data.User>> UpdateUserAsync(Core.Models.Data.User user);



        #region Organization Access


        Task<List<Models.Data.AspNetUserOrganization>> GetUserOrganizationsByUserIdAsync(int UserId);
        Task<Result<bool>> UpsertUserOrganizationsAsync(int UserId, List<int> OrganizationIds);
        #endregion
    }
}
