namespace KONGOR.Shared.Handlers.Client;

public class ShowSimpleStatsHandler : IClientRequestHandler
{
	private readonly IPlayerStatsService _playerStatsService;

	public ShowSimpleStatsHandler(IPlayerStatsService playerStatsService)
	{
		_playerStatsService = playerStatsService;
	}

	public async Task<IActionResult> HandleRequest(Dictionary<string, string> formData)
	{
		string cookie = formData["cookie"];
		string nickname = formData["nickname"];

		// Use the service to get stats
		var data = await _playerStatsService.GetShowSimpleStatsAsync(nickname, cookie);

		if (data is null)
		{
			return new NotFoundResult();
		}

		// TODO: include awards once we have stats.
		ShowSimpleStatsResponse response = new ShowSimpleStatsResponse(
			nickname,
			data.TotalLevel,
			data.TotalExperience,
			data.NumberOfHeroesOwned,
			data.UnlockedUpgradeCodes.Count(up => up.StartsWith("aa.")),
			data.TotalMatchesPlayed,
			data.CombinedPlayerAwardSummary.MVP,
			data.SelectedUpgradeCodes,
			data.AccountId,
			data.SeasonId,
			data.SeasonNormal,
			data.SeasonCasual,
			data.CombinedPlayerAwardSummary.Top4Names,
			data.CombinedPlayerAwardSummary.Top4Nums
		);

		return new OkObjectResult(PHP.Serialize(response));
	}
}
