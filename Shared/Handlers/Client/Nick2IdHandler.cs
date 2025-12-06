namespace KONGOR.Shared.Handlers.Client;

public class Nick2IdHandler : IClientRequestHandler
{
	private readonly IAccountService _accountService;

	public Nick2IdHandler(IAccountService accountService)
	{
		_accountService = accountService;
	}

	public async Task<IActionResult> HandleRequest(Dictionary<string, string> formData)
	{
		// Extract usernames from formData
		IEnumerable<string> usernames = formData.Values.ToList();

		// Skip the first value if the "f" key exists
		if (formData.ContainsKey("f"))
		{
			usernames = usernames.Skip(1);
		}

		var output = await _accountService.GetAccountIdsAsync(usernames);

		if (!output.Any())
		{
			return new NotFoundObjectResult(
				new AccountLookupErrorResponse(AccountLookupErrorResponseType.NoAccountNameMatches)
			);
		}

		// Add constants as needed
		output.Add("vested_threshold", 5);
		output.Add("0", 1);

		return new OkObjectResult(PhpSerialization.Serialize(output));
	}
}