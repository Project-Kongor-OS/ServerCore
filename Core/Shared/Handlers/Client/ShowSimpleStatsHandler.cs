namespace KONGOR.Shared.Handlers.Client;

public class ShowSimpleStatsHandler : IClientRequestHandler
{
	private readonly IAuthService _authService;
	private readonly IStatsService _statsService;

	public ShowSimpleStatsHandler(IStatsService playerStatsService, IAuthService authService)
	{
		_authService = authService;
		_statsService = playerStatsService;
	}

	public async Task<IActionResult> HandleRequest(Dictionary<string, string> formData)
	{
		string cookie = formData["cookie"];

		bool valid_cookie = await _authService.IsValidCookieAsync(cookie);
		if (!valid_cookie)
		{
			return new UnauthorizedResult();
		}

		string nickname = formData["nickname"];

		// Use the service to get stats
		var data = await _statsService.GetShowSimpleStatsAsync(nickname);	

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
