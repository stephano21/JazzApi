using JazzApi.Services.MailService;

namespace JazzApi.Interfaces
{
    public interface IMailRepository
    {
        Task<MailRepositoryResponse> SendEmail(List<string> toEmails, string subject, string htmlContent, Dictionary<string, byte[]> attachments = null);
        string LoadEmailTemplate(string TemplateName);
        string PopulateTemplate(string template, Dictionary<string, string> placeholders);
    }
}
