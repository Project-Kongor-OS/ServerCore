namespace ProjectKongor.Tests.Handlers.Client;

public class MatchHistoryOverviewHandlerTests
{
	private MatchHistoryOverviewHandler CreateHandler(
		out Mock<IStatsService> statsMock,
		out Mock<IAuthService> authMock,
		string? serializedMatchIds = "1|2|3",
		Dictionary<string, string>? matchOverview = null,
		bool cookieValid = true)
	{
		statsMock = new Mock<IStatsService>();
		authMock = new Mock<IAuthService>();

		// Cookie validation behavior
		authMock
			.Setup(a => a.IsValidCookieAsync(It.IsAny<string>()))
			.ReturnsAsync(cookieValid);

		// Serialized match IDs behavior
		statsMock
			.Setup(s => s.GetSerializedMatchIdsAsync(It.IsAny<string>(), It.IsAny<string>()))
			.ReturnsAsync(serializedMatchIds);

		// Match history summary behavior
		statsMock
			.Setup(s => s.GetMatchHistoryOverviewAsync(
				It.IsAny<string>(),
				It.IsAny<string>(),
				It.IsAny<int>()))
			.ReturnsAsync(matchOverview ?? new Dictionary<string, string>
			{
				["m0"] = "1,1,1,10,2,3,4,500,caldavar,normal,clientA",
				["m1"] = "2,0,2,6,5,2,7,450,caldavar,normal,clientB",
			});

		return new MatchHistoryOverviewHandler(statsMock.Object, authMock.Object);
	}

	[Fact]
	public async Task HandleRequest_ReturnsUnauthorized_WhenCookieInvalid()
	{
		// Arrange
		var handler = CreateHandler(
			out var statsMock,
			out var authMock,
			serializedMatchIds: null,
			cookieValid: false);

		var formData = new Dictionary<string, string>
		{
			["cookie"] = "bad_cookie",
			["nickname"] = "player1",
			["table"] = "player",
			["num"] = "10"
		};

		// Act
		var result = await handler.HandleRequest(formData);

		// Assert
		Assert.IsType<UnauthorizedResult>(result);

		authMock.Verify(a => a.IsValidCookieAsync("bad_cookie"), Times.Once);
		statsMock.Verify(s => s.GetSerializedMatchIdsAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
	}

	[Fact]
	public async Task HandleRequest_ReturnsNotFound_WhenSerializedMatchIdsIsNull()
	{
		// Arrange
		var handler = CreateHandler(
			out var statsMock,
			out var authMock,
			serializedMatchIds: null,
			cookieValid: true);

		var formData = new Dictionary<string, string>
		{
			["cookie"] = "valid_cookie",
			["nickname"] = "player1",
			["table"] = "player",
			["num"] = "10"
		};

		// Act
		var result = await handler.HandleRequest(formData);

		// Assert
		Assert.IsType<NotFoundResult>(result);

		statsMock.Verify(s => s.GetSerializedMatchIdsAsync("player1", "player"), Times.Once);
		statsMock.Verify(s =>
			s.GetMatchHistoryOverviewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()),
			Times.Never);
	}

	[Fact]
	public async Task HandleRequest_ReturnsOk_WithSerializedMatches_WhenCookieValid()
	{
		// Arrange
		var fakeMatches = new Dictionary<string, string>
		{
			["m0"] = "1,1,1,10,2,3,4,500,caldavar,normal,clientA",
			["m1"] = "2,0,2,6,5,2,7,450,caldavar,normal,clientB",
		};

		var handler = CreateHandler(
			out var statsMock,
			out var authMock,
			serializedMatchIds: "1|2",
			matchOverview: fakeMatches,
			cookieValid: true);

		var formData = new Dictionary<string, string>
		{
			["cookie"] = "good_cookie",
			["nickname"] = "player1",
			["table"] = "player",
			["num"] = "2"
		};

		// Act
		var result = await handler.HandleRequest(formData);

		// Assert
		var ok = Assert.IsType<OkObjectResult>(result);
		var serialized = Assert.IsType<string>(ok.Value);

		var expected = @"a:2:{s:2:""m0"";s:42:""1,1,1,10,2,3,4,500,caldavar,normal,clientA"";s:2:""m1"";s:41:""2,0,2,6,5,2,7,450,caldavar,normal,clientB"";}";
		Assert.Equal(expected, serialized);

		statsMock.Verify(s => s.GetSerializedMatchIdsAsync("player1", "player"), Times.Once);
		// Verifies that the number of matches (2) was parsed correctly.
		statsMock.Verify(s => s.GetMatchHistoryOverviewAsync("player1", "1|2", 2), Times.Once);
	}
}
