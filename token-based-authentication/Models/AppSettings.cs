namespace token_based_authentication.Models
{
    public class AppSettings
    {
        public PublicKey[] PublicKey { get; set; }
    }

    public class PublicKey
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string[] Urls { get; set; }
    }
}
