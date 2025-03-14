using Ardalis.Result;
using Core.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
    public interface IUser : IGenericRepository<Models.Data.User>
    {
        //Task<Core.Models.Data.User?> GetByIdAsync(int id);
        Task<Core.Models.Data.User> GetUserByPublicKeyAsync(Guid publicKey);

       
    }
}
