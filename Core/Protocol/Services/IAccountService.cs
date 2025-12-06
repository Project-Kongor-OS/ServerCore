namespace ProjectKongor.Protocol.Services;

public interface IAccountService
{
	/// <summary>
	/// Returns a dictionary mapping usernames to account IDs.
	/// </summary>
	Task<Dictionary<string, int>> GetAccountIdsAsync(IEnumerable<string> usernames);
}