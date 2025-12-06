
using KONGOR.Shared.Handlers.Client;

namespace ServerCoreTest;

public class Nick2IdHandlerTests
{
	[Fact]
	public async Task HandleRequest_NoAccountsFound_ReturnsNotFound()
	{
		// Arrange
		var mockAccountService = new Mock<IAccountService>();
		mockAccountService
			.Setup(s => s.GetAccountIdsAsync(It.IsAny<IEnumerable<string>>()))
			.ReturnsAsync(new Dictionary<string, int>());

		var handler = new Nick2IdHandler(mockAccountService.Object);

		var formData = new Dictionary<string, string>
		{
			{ "f", "nick2id" },
			{ "nickname[0]", "unknownuser" }
		};

		// Act
		var result = await handler.HandleRequest(formData);

		// Assert
		var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
		var errorResponse = Assert.IsType<AccountLookupErrorResponse>(notFoundResult.Value);
		Assert.Equal("No Account Name(s) Found", errorResponse.Error);
	}

	[Fact]
	public async Task HandleRequest_AccountsFound_ReturnsOk()
	{
		// Arrange
		var mockAccountService = new Mock<IAccountService>();
		var accountDict = new Dictionary<string, int> { { "user1", 123 }, { "user2", 456 } };
		mockAccountService
			.Setup(s => s.GetAccountIdsAsync(It.IsAny<IEnumerable<string>>()))
			.ReturnsAsync(accountDict);

		var handler = new Nick2IdHandler(mockAccountService.Object);

		var formData = new Dictionary<string, string>
		{
			{ "nickname[0]", "user1" },
			{ "nickname[1]", "user2" }
		};

		// Act
		var result = await handler.HandleRequest(formData);

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);
		var serializedOutput = Assert.IsType<string>(okResult.Value);

		// Optionally, deserialize the output to check contents
		var deserialized = PhpSerialization.Deserialize<Dictionary<string, object>>(serializedOutput);
		Assert.Equal(123, (int)deserialized["user1"]);
		Assert.Equal(456, (int)deserialized["user2"]);
		Assert.Equal(5, (int)deserialized["vested_threshold"]);
		Assert.Equal(1, (int)deserialized["0"]);
	}
}
