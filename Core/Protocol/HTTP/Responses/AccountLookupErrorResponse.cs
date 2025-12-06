namespace ProjectKongor.Protocol.HTTP.Responses;

public enum AccountLookupErrorResponseType
{
	NoAutoCompleteMatches,
	NoAccountNameMatches
}

public class AccountLookupErrorResponse
{
	public AccountLookupErrorResponse(AccountLookupErrorResponseType errorType)
	{
		Error = errorType switch
		{
			AccountLookupErrorResponseType.NoAutoCompleteMatches => "FALSE",
			AccountLookupErrorResponseType.NoAccountNameMatches => "No Account Name(s) Found",
			_ => string.Empty
		};
	}

	/// <summary>
	///     The error message for the response.
	/// </summary>
	[PhpProperty("error")]
	public readonly string Error = "FALSE";

	/// <summary>
	///     Unknown property which seems to be set to "0" (false) on an error response.
	/// </summary>
	[PhpProperty(0)]
	public readonly bool Zero = false;
}