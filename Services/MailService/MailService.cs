using System.Net.Mail;
using System.Net;

namespace JazzApi.Services.MailService
{
    public class MailService
    {
        public class MailRepository
        {
            private readonly SmtpClient _mailClient;
            private readonly string _username;

            public MailRepository(IConfiguration configuration)
            {
                string password;
                string host;
                int port;
                bool ssl;

                if ((Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? configuration["Env"]) == "Production")
                {
                    _username = configuration["MailService:Email"] ?? Environment.GetEnvironmentVariable("Email");
                    password = configuration["MailService:Password"] ?? Environment.GetEnvironmentVariable("Password");
                    host = configuration["MailService:Host"] ?? Environment.GetEnvironmentVariable("Host");
                    port = int.Parse(configuration["MailService:Port"] ?? Environment.GetEnvironmentVariable("Port"));
                    ssl = true;
                }
                else
                {
                    _username = configuration["MailService:Email"] ?? Environment.GetEnvironmentVariable("Email");
                    password = configuration["MailService:Password"] ?? Environment.GetEnvironmentVariable("Password");
                    host = configuration["MailService:Host"] ?? Environment.GetEnvironmentVariable("Host");
                    port = int.Parse(configuration["MailService:Port"] ?? Environment.GetEnvironmentVariable("Port"));
                    ssl = true;
                }

                _mailClient = new SmtpClient
                {
                    Credentials = new NetworkCredential(_username, password),
                    EnableSsl = ssl,
                    Host = host,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Port = port
                };
            }

            public async Task<MailRepositoryResponse> SendEmail(List<string> toEmails, string subject, string htmlContent, Dictionary<string, byte[]> attachments = null)
            {
                try
                {
                    var emailMessage = new MailMessage
                    {
                        From = new MailAddress(_username, "Jazz Notify"),
                        Subject = subject,
                        Body = htmlContent,
                        IsBodyHtml = true
                    };

                    if (attachments != null)
                    {
                        foreach (var attachment in attachments)
                        {
                            var attachmentObject = new Attachment(new MemoryStream(attachment.Value), attachment.Key);
                            emailMessage.Attachments.Add(attachmentObject);
                        }
                    }

                    foreach (var toEmail in toEmails)
                    {
                        emailMessage.To.Add(toEmail);
                    }

                    await _mailClient.SendMailAsync(emailMessage);

                    return new MailRepositoryResponse { Successful = true, Message = "Correo enviado correctamente" };
                }
                catch (Exception e)
                {
                    return new MailRepositoryResponse { Successful = false, Message = e.Message };
                }
            }
            public string LoadEmailTemplate()
            {
                var basePath = Directory.GetCurrentDirectory();
                var templatePath = Path.GetFullPath(Path.Combine(basePath, "Templates", "ConfirmEmailTemplate.html"));
                var template = File.ReadAllText(templatePath);
                return template;
            }
            public string PopulateTemplate(string template, Dictionary<string, string> placeholders)
            {
                foreach (var placeholder in placeholders)
                {
                    template = template.Replace($"{{{placeholder.Key}}}", placeholder.Value);
                }
                return template;
            }

        }
    }
}
