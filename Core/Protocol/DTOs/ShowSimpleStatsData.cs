namespace ProjectKongor.Protocol.DTOs;

public record ShowSimpleStatsData(
	int TotalLevel,
	int TotalExperience,
	int NumberOfHeroesOwned,
	int TotalMatchesPlayed,
	CombinedPlayerAwardSummary CombinedPlayerAwardSummary,
	ICollection<string> SelectedUpgradeCodes,
	ICollection<string> UnlockedUpgradeCodes,
	int AccountId,
	int SeasonId,
	SeasonShortSummary SeasonNormal,
	SeasonShortSummary SeasonCasual);