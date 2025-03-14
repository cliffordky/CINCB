using Ardalis.Result;
using Core;
using Core.Models.Configuration;
using Core.Repository;
using FluentEmail.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IFluentEmail _fluentEmail;
        private readonly IMessageTemplate _messageTemplateRepository;
        private readonly EmailSettings _emailSettings;

        public EmailService(ILogger<EmailService> logger, IOptions<EmailSettings> options, IFluentEmail fluentEmail, Core.Repository.IMessageTemplate messageTemplate)
        {
            _logger = logger;
            _fluentEmail = fluentEmail;
            _messageTemplateRepository = messageTemplate;
            _emailSettings = options.Value;
        }

        private enum MessageTemplate
        {
            RESET_PASSWORD_EMAIL = 1,
            CONFIRMATION_EMAIL = 2
        }

        private async Task<Core.Models.Data.MessageTemplate> GetMessageTemplateAsync(MessageTemplate TemplateToRetrieve)
        {
            return await _messageTemplateRepository.GetByIdAsync((int)TemplateToRetrieve);
        }

        public async Task SendEmailAsync(string email, string subject, string content)
        {
            await Execute(subject, content, email);
        }

        public async Task<Result<bool>> SendPasswordResetEmailAsync(string To, string callbackUrl)
        {
            try
            {
                var template = await GetMessageTemplateAsync(MessageTemplate.RESET_PASSWORD_EMAIL);
                if (template == null) return Result<bool>.Error();

                var model = new Core.Models.Communication.ResetPassword() { EmailAddress = To, CallBackUrl = callbackUrl };
                var sendResult = await _fluentEmail
                 .UsingTemplate(template.Body, model)
                 .To(To)
                 .Subject(template.Subject)
                 .Tag(Guid.NewGuid().ToString())
                 .SendAsync();

                return sendResult.Successful
                    ? Result<bool>.Success(true)
                    : Result<bool>.Error();
            }
            catch (Exception Ex)
            {
                return Result<bool>.Error();
            }
        }

        public async Task<Result<bool>> SendTemplateEmail(string To)
        {
            var template = await GetMessageTemplateAsync(MessageTemplate.RESET_PASSWORD_EMAIL);
            if (template == null) return Result<bool>.Error();
            var model = new Core.Models.Data.User() { FirstName = "Clifford" };

            var sendResult = await _fluentEmail
               .UsingTemplate(template.Body, model)
                 .To(To)
                .Subject(template.Subject)
                 .Tag(Guid.NewGuid().ToString())
                 .SendAsync();

            return sendResult.Successful
                ? Result<bool>.Success(true)
                : Result<bool>.Error();
        }

        private async Task Execute(string subject, string message, string To)
        {
            var client = new SendGridClient(_emailSettings.ApiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(To));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);
            _logger.LogInformation(response.IsSuccessStatusCode
                                   ? $"Email to {To} queued successfully!"
                                   : $"Failure Email to {To}");
        }

        public async Task<Result<bool>> SendConfirmationEmailAsync(string To, string callbackUrl)
        {
            try
            {
                var template = await GetMessageTemplateAsync(MessageTemplate.CONFIRMATION_EMAIL);
                if (template == null) return Result<bool>.Error();

                var model = new Core.Models.Communication.ConfirmAccount() { EmailAddress = To, CallBackUrl = callbackUrl };
                var sendResult = await _fluentEmail
                 .UsingTemplate(template.Body, model)
                 .To(To)
                 .Subject(template.Subject)
                 .Tag(Guid.NewGuid().ToString())
                 .SendAsync();

                return sendResult.Successful
                    ? Result<bool>.Success(true)
                    : Result<bool>.Error();
            }
            catch (Exception Ex)
            {
                return Result<bool>.Error();
            }
        }
    }
}