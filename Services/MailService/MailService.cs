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

                if ((configuration["Env"]?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")) == "Production")
                {
                    _username = "trabajocolaborativo.pis@gmail.com";
                    password = "liou cfwa hgbz ndeu";
                    host = "smtp.gmail.com";
                    port = 587;
                    ssl = true;
                }
                else
                {
                    _username = "trabajocolaborativo.pis@gmail.com";
                    password = "liou cfwa hgbz ndeu";
                    host = "smtp.gmail.com";
                    port = 587;
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
        }
    }
}
