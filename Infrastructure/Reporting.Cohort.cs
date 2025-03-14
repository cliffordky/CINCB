using Ardalis.Result;
using Core.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public partial class Reporting
    {
        public async Task<IReadOnlyList<Cohort>> GetCohortsAsync()
        {
            try
            {
                return await _cohortRepository.GetAllAsync();
            }
            catch (Exception Ex)
            {
                return new List<Core.Models.Data.Cohort>();
            }
        }

        public async Task<Result<Cohort>> GetCohortByIdAsync(int Id)
        {
            try
            {
                var item = await _cohortRepository.GetByIdAsync(Id);
                if (item == null)
                {
                    return Result<Core.Models.Data.Cohort>.Error($"Menu ({Id}) not found");
                }

                return item;
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.Cohort>.Error(Ex.Message);
            }
        }

        public async Task<Result<Cohort>> UpdateCohortAsync(Cohort item)
        {
            try
            {
                var result = await _cohortRepository.UpdateAsync(item);
                if (result == null)
                {
                    return Result<Core.Models.Data.Cohort>.Error("Error updating menu");
                }

                return Result.Success(result);
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.Cohort>.Error(Ex.Message);
            }
        }

        public async Task<Result<Cohort>> AddCohortAsync(Cohort item)
        {
            try
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.IsDeleted = false;
                item.AllowUserDelete = true;
                item.Sequence = 0;

                var result = await _cohortRepository.AddAsync(item);
                if (result == null)
                {
                    return Result<Core.Models.Data.Cohort>.Error($"Error adding menu ({item.Name})");
                }

                return Result.Success(result);
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.Cohort>.Error(Ex.Message);
            }
        }

        public async Task<Result<bool>> DeleteCohortAsync(int Id)
        {
            try
            {
                var result = await _cohortRepository.DeleteAsync(Id);
                if (!result)
                {
                    return Result<bool>.Error($"Error deleting menu ({Id})");
                }

                return Result.Success(true);
            }
            catch (Exception Ex)
            {
                return Result<bool>.Error(Ex.Message);
            }
        }


        public async Task<Result<CohortFilter>> GetCohortFilterByIdAsync(int Id)
        {
            try
            {
                var item = await _cohortFilterRepository.GetByIdAsync(Id);
                if (item == null)
                {
                    return Result<Core.Models.Data.CohortFilter>.Error($"Cohort Filter ({Id}) not found");
                }

                return item;
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.CohortFilter>.Error(Ex.Message);
            }
        }
        public async Task<IReadOnlyList<CohortFilter>> GetCohortFiltersByCohortIdAsync(int cohortId)
        {
            try
            {
                return await _cohortFilterRepository.GetCohortFiltersByCohortIdAsync(cohortId);
            }
            catch (Exception Ex)
            {
                return new List<Core.Models.Data.CohortFilter>();
            }
        }

        public async Task<Result<CohortFilter>> UpdateCohortFilterAsync(CohortFilter item)
        {
            try
            {
                var result = await _cohortFilterRepository.UpdateAsync(item);
                if (result == null)
                {
                    return Result<Core.Models.Data.CohortFilter>.Error("Error updating menu item");
                }

                return Result.Success(result);
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.CohortFilter>.Error(Ex.Message);
            }
        }

        public async Task<Result<CohortFilter>> AddCohortFilterAsync(CohortFilter item)
        {
            try
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.IsDeleted = false;
                item.AllowUserDelete = true;
                item.Sequence = 0;

                var result = await _cohortFilterRepository.AddAsync(item);
                if (result == null)
                {
                    return Result<Core.Models.Data.CohortFilter>.Error($"Error adding item ({item.Id})");
                }

                return Result.Success(result);
            }
            catch (Exception Ex)
            {
                return Result<Core.Models.Data.CohortFilter>.Error(Ex.Message);
            }
        }

        public async Task<Result<bool>> DeleteCohortFilterAsync(int Id)
        {
            try
            {
                var result = await _cohortFilterRepository.DeleteAsync(Id);
                if (!result)
                {
                    return Result<bool>.Error($"Error deleting menu item ({Id})");
                }

                return Result.Success(true);
            }
            catch (Exception Ex)
            {
                return Result<bool>.Error(Ex.Message);
            }
        }
    }
}