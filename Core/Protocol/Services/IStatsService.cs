namespace ProjectKongor.Protocol.Services;

public interface IPlayerStatsService
{
	Task<ShowSimpleStatsData?> GetShowSimpleStatsAsync(string nickname, string cookie);
}