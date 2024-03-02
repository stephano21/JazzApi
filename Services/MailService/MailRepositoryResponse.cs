using System.Text.Json.Serialization;

namespace JazzApi.Services.MailService
{
    public class MailRepositoryResponse
    {
        [JsonPropertyName("succesful")]
        public bool Successful { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }


        [JsonPropertyName("reqNumber")]
        public string reqNumber { get; set; } = "";
    }
}
