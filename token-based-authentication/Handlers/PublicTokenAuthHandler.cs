using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using token_based_authentication.Models;

namespace token_based_authentication.HHandlers;

// Bu sınıf, Public Token kimlik doğrulama sürecini gerçekleştiren özel bir kimlik doğrulama yöneticisidir.
public class PublicTokenAuthHandler : AuthenticationHandler<PublicTokenAuthOptions>
{
    private AppSettings _appSettings;

    public PublicTokenAuthHandler(IOptions<AppSettings> appSettings, IOptionsMonitor<PublicTokenAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock) => _appSettings = appSettings.Value;

    // Bu metod, gelen bir HTTP isteğinin kimlik doğrulamasını gerçekleştirir.
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            // İstek başlığında beklenen token’ın olup olmadığını kontrol eder.
            if (!Request.Headers.ContainsKey(Options.TokenHeaderName))
                return Task.FromResult(AuthenticateResult.Fail($"Missing Header For Token: {Options.TokenHeaderName}"));

            // Başlıktan token’ı alır.
            var token = Request.Headers[Options.TokenHeaderName];

            // Yapılandırma ayarlarında belirtilen token ile eşleşen bir public key olup olmadığını kontrol eder.
            var key = _appSettings.PublicKey.FirstOrDefault(p => p.Token.ToUpper().Equals(token.ToString().ToUpper()));

            // Token ile eşleşen bir anahtar olup olmadığını kontrol eder.
            if (key == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Token not authorized"));
            }

            // Token'ın erişim izni olan URL'lere erişip erişmediğini kontrol eder.
            if (!key.Urls.Contains(Request.Path.Value))
            {
                return Task.FromResult(AuthenticateResult.Fail("Token has no access privileges"));
            }

            // Eğer token doğru ve URL'e erişim izni varsa, ilgili claims nesnelerini oluşturur.
            var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, "Username"),
            new Claim(ClaimTypes.Name, key.Username)
        };
            var id = new ClaimsIdentity(claims, Scheme.Name);

            // ClaimsIdentity objesini bir ClaimsPrincipal içine yerleştirir ve onunla bir kimlik doğrulama bileti oluşturur.
            var principal = new ClaimsPrincipal(id);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            // Başarılı bir kimlik doğrulama sonucu döndürür.
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}

// Bu sınıf, Public Token kimlik doğrulama şeması ile ilgili yapılandırma seçeneklerini tanımlar.
public class PublicTokenAuthOptions : AuthenticationSchemeOptions
{
    public const string DefaultSchemeName = "PublicTokenAuthenticationScheme";
    public string TokenHeaderName { get; set; } = "projectToken";
}
