namespace ProjectKongor.Protocol.Handlers.Client;

public class MatchHistoryOverviewHandler : IClientRequestHandler
{
	private readonly IStatsService _statsService;
	private readonly IAuthService _authService;

	public MatchHistoryOverviewHandler(IStatsService statsService, IAuthService authService)
	{
		_authService = authService;
		_statsService = statsService;
	}

	public async Task<IActionResult> HandleRequest(
		Dictionary<string, string> formData)
	{
		string cookie = formData["cookie"];

		bool valid_cookie = await _authService.IsValidCookieAsync(cookie);
		if (!valid_cookie)
		{
			return new UnauthorizedResult();
		}

		string nickname = formData["nickname"];
		string table = formData["table"];

		string? serializedMatchIds =
			await _statsService.GetSerializedMatchIdsAsync(nickname, table);

		if (serializedMatchIds == null)
		{
			return new NotFoundResult();
		}

		int numberOfMatchesToRetrieve = int.Parse(formData["num"]);

		var matches = await _statsService.GetMatchHistoryOverviewAsync(
			nickname,
			serializedMatchIds,
			numberOfMatchesToRetrieve);

		return new OkObjectResult(PHP.Serialize(matches));
	}
}
