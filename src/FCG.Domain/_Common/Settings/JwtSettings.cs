namespace FCG.Domain._Common.Settings;
public class JwtSettings
{
    public string Secret { get; set; }
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public int ExpirationInHours { get; set; }
}