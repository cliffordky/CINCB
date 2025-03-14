using Ardalis.Result;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository
{
    public interface IReporting
    {
        Task<Result<Core.Models.Patient>> GetPatientDataAsync(string identifier);


    }
}
