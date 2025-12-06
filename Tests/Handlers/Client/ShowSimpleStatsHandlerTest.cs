namespace ProjectKongor.Tests.Handlers.Client;

public class ShowSimpleStatsHandlerTests
{
	private ShowSimpleStatsHandler CreateHandler(out Mock<IPlayerStatsService> serviceMock, ShowSimpleStatsData? returnData = null)
	{
		serviceMock = new Mock<IPlayerStatsService>();
		serviceMock
			.Setup(s => s.GetShowSimpleStatsAsync(It.IsAny<string>(), It.IsAny<string>()))
			.ReturnsAsync(returnData);

		return new ShowSimpleStatsHandler(serviceMock.Object);
	}

	[Fact]
	public async Task HandleRequest_ReturnsOk_WhenDataExists()
	{
		// Arrange
		var dto = new ShowSimpleStatsData(
			TotalLevel: 10,
			TotalExperience: 5000,
			NumberOfHeroesOwned: 139,
			TotalMatchesPlayed: 100,
			CombinedPlayerAwardSummary: new CombinedPlayerAwardSummary(
				3,
				new List<string> { "a", "b", "c", "d" },
				new List<int> { 5, 4, 3, 2 }
			),
			SelectedUpgradeCodes: new List<string> { "upgrade1", "upgrade2" },
			UnlockedUpgradeCodes: new List<string> { "aa.up1", "aa.up2", "bb.up3" },
			AccountId: 42,
			SeasonId: 22,
			SeasonNormal: new SeasonShortSummary(10, 5, 2, 1),
			SeasonCasual: new SeasonShortSummary(3, 1, 0, 0)
		);

		var handler = CreateHandler(out var mockService, dto);

		var formData = new Dictionary<string, string>
		{
			["cookie"] = "test_cookie",
			["nickname"] = "player1"
		};

		// Act
		var result = await handler.HandleRequest(formData);

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);
		var serialized = Assert.IsType<string>(okResult.Value);

		// Verify
		var expected =
			@"a:14:{s:8:""nickname"";s:7:""player1"";s:5:""level"";i:10;s:9:""level_exp"";i:5000;s:8:""hero_num"";i:139;s:10:""avatar_num"";i:2;s:12:""total_played"";i:100;s:7:""mvp_num"";i:3;s:17:""selected_upgrades"";a:2:{i:0;s:8:""upgrade1"";i:1;s:8:""upgrade2"";}s:10:""account_id"";i:42;s:9:""season_id"";i:22;s:13:""season_normal"";a:4:{s:4:""wins"";i:10;s:6:""losses"";i:5;s:10:""win_streak"";i:2;s:13:""current_level"";i:1;}s:13:""season_casual"";a:4:{s:4:""wins"";i:3;s:6:""losses"";i:1;s:10:""win_streak"";i:0;s:13:""current_level"";i:0;}s:15:""award_top4_name"";a:4:{i:0;s:1:""a"";i:1;s:1:""b"";i:2;s:1:""c"";i:3;s:1:""d"";}s:14:""award_top4_num"";a:4:{i:0;i:5;i:1;i:4;i:2;i:3;i:3;i:2;}}";
		Assert.Equal(expected, serialized);
	}

	[Fact]
	public async Task HandleRequest_ReturnsNotFound_WhenDataIsNull()
	{
		// Arrange
		var handler = CreateHandler(out var mockService, returnData: null);

		var formData = new Dictionary<string, string>
		{
			["cookie"] = "test_cookie",
			["nickname"] = "player1"
		};

		// Act
		var result = await handler.HandleRequest(formData);

		// Assert
		Assert.IsType<NotFoundResult>(result);
	}
}
