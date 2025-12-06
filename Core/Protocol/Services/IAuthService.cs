namespace ProjectKongor.Protocol.Services;

public interface IAuthService
{
	Task<bool> IsValidCookieAsync(string cookie);
}