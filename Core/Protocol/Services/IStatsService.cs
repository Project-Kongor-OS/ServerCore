namespace ProjectKongor.Protocol.Services;

/// <summary>
///     Provides functionality for retrieving player statistics, match history data,
///     and other stats-related information for the Project Kongor ecosystem.
/// </summary>
public interface IStatsService
{
	Task<ShowSimpleStatsData?> GetShowSimpleStatsAsync(string nickname);

	Task<string?> GetSerializedMatchIdsAsync(string nickname, string table);

	Task<Dictionary<string, string>> GetMatchHistoryOverviewAsync(
		string nickname,
		string serializedMatchIds,
		int numberOfMatches);
}