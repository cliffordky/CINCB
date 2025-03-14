using Ardalis.Result;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Integrations
{
    public interface IOpenAI
    {
        Task<Result<string>> GenerateCronExpression(string prompt);
    }
}