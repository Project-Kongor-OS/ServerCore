namespace ProjectKongor.Protocol.DTOs;

public record PlayerAwardSummary(
	int TopAnnihilations,
	int MostQuadKills,
	int BestKillStreak,
	int MostSmackdowns,
	int MostKills,
	int MostAssists,
	int LeastDeaths,
	int TopSiegeDamage,
	int MostWardsKilled,
	int TopHeroDamage,
	int TopCreepScore,
	int MVP
);