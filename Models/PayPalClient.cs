using System.ComponentModel.DataAnnotations;

namespace TBD.Models
{
    public class PayPalClient
    {
        public string Mode { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public PayPalClient(string mode, string clientId, string clientSecret) {
            Mode = mode;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
    }
}
