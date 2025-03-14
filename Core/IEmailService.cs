using Ardalis.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public interface IEmailService
    {
        Task<Result<bool>> SendConfirmationEmailAsync(string To, string callbackUrl);
        Task SendEmailAsync(string email, string subject, string content);

        Task<Result<bool>> SendPasswordResetEmailAsync(string To, string callbackUrl);
        Task<Result<bool>> SendTemplateEmail(string To);
    }
}