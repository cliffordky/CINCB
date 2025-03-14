using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
    public interface IAspNetUserOrganization : IGenericRepository<Models.Data.AspNetUserOrganization>
    {
        Task<IReadOnlyList<Core.Models.Data.AspNetUserOrganization>> GetAllByUserIdAsync(int UserId);
    }
}
