using System.Globalization;
using castledice_game_data_logic;
using castledice_game_logic;
using castledice_game_server.GameRepository;
using castledice_game_server.GameService;
using static castledice_game_server_tests.ObjectCreationUtility;
using Moq;

namespace castledice_game_server_tests.GameServiceTests;

public class HttpGameSavingServiceTests
{
    [Theory]
    [InlineData("someconfig")]
    [InlineData("someotherconfig")]
    [InlineData("someotherotherconfig")]
    public async void SaveGameStartAsync_ShouldPostGameData_WithConfigFromGameStartDataJsonConverter(
        string expectedConfig)
    {
        var dataSenderMock = GetHttpRepositoryMock();
        var jsonConverterMock = GetJsonConverterMock();
        jsonConverterMock.Setup(x => x.GetJson(It.IsAny<GameStartData>())).Returns(expectedConfig);
        var service = new HttpGameSavingService(dataSenderMock.Object, GetTimeProviderMock().Object,
            jsonConverterMock.Object, GetLocalRepositoryMock().Object);
        Predicate<GameData> gameDataHasNeededString = gameData => gameData.Config == expectedConfig;

        await service.SaveGameStartAsync(GetGameStartData());

        dataSenderMock.Verify(x => x.PostGameDataAsync(It.Is<GameData>(g => gameDataHasNeededString(g))), Times.Once);
    }

    [Theory]
    [InlineData("2/27/2023 2:06:49")]
    [InlineData("2/15/2022 2:06:50")]
    [InlineData("3/29/2023 2:06:51")]
    public async void SaveGameStartAsync_ShouldPostGameData_WithGameStartedTimeFromGivenCurrentTimeProvider(
        string expectedTimeStr)
    {
        var expectedTime = DateTime.Parse(expectedTimeStr, CultureInfo.InvariantCulture);
        var dataSenderMock = GetHttpRepositoryMock();
        var timeProviderMock = GetTimeProviderMock();
        timeProviderMock.Setup(x => x.GetCurrentTime()).Returns(expectedTime);
        var service = new HttpGameSavingService(dataSenderMock.Object, timeProviderMock.Object,
            GetJsonConverterMock().Object, GetLocalRepositoryMock().Object);
        Predicate<GameData> gameDataHasNeededTime = gameData => gameData.GameStartedTime == expectedTime;

        await service.SaveGameStartAsync(GetGameStartData());

        dataSenderMock.Verify(x => x.PostGameDataAsync(It.Is<GameData>(g => gameDataHasNeededTime(g))), Times.Once);
    }

    [Fact]
    public async void SaveGameStartAsync_ShouldPostGameData_WithPlayersListFromGivenGameStartData()
    {
        var gameStartData = GetGameStartData();
        var dataSenderMock = GetHttpRepositoryMock();
        var service = new HttpGameSavingService(dataSenderMock.Object, GetTimeProviderMock().Object,
            GetJsonConverterMock().Object, GetLocalRepositoryMock().Object);
        Predicate<GameData> gameDataHasNeededPlayersList = gameData => gameData.Players.Equals(gameStartData.PlayersIds);
        
        await service.SaveGameStartAsync(gameStartData);

        dataSenderMock.Verify(x => x.PostGameDataAsync(It.Is<GameData>(g => gameDataHasNeededPlayersList(g))), Times.Once);
    }

    [Fact]
    public async void SaveGameStartAsync_ShouldWrapHttpRequestException_IfCaughtOne()
    {
        var exception = new HttpRequestException();
        var dataSenderMock = new Mock<IHttpGameDataRepository>();
        dataSenderMock.Setup(x => x.PostGameDataAsync(It.IsAny<GameData>())).ThrowsAsync(exception);
        var service = new HttpGameSavingService(dataSenderMock.Object, GetTimeProviderMock().Object,
            GetJsonConverterMock().Object, GetLocalRepositoryMock().Object);
        
        var result = await Record.ExceptionAsync(() => service.SaveGameStartAsync(GetGameStartData()));
        
        Assert.Same(exception, result.InnerException);
    }

    [Theory]
    [InlineData("someconfig", "2/27/2023 2:06:49")]
    [InlineData("someotherconfig", "2/15/2022 2:06:50")]
    [InlineData("someotherotherconfig", "3/29/2023 2:06:51")]
    public async void SaveGameStartAsync_ShouldAddAppropriateGameDataToLocalRepository(string expectedConfig, string expectedDateTimeStr)
    {
        var expectedDateTime = DateTime.Parse(expectedDateTimeStr, CultureInfo.InvariantCulture);
        var gameStartData = GetGameStartData();
        var expectedPlayers = gameStartData.PlayersIds;
        var converterMock = GetJsonConverterMock();
        converterMock.Setup(x => x.GetJson(gameStartData)).Returns(expectedConfig);
        var timeProviderMock = GetTimeProviderMock();
        timeProviderMock.Setup(x => x.GetCurrentTime()).Returns(expectedDateTime);
        var localRepositoryMock = GetLocalRepositoryMock();
        var service = new HttpGameSavingService(GetHttpRepositoryMock().Object, timeProviderMock.Object,
            converterMock.Object, localRepositoryMock.Object);
        Predicate<GameData> gameDataIsAppropriate = gameData => gameData.Config == expectedConfig && gameData.GameStartedTime == expectedDateTime && gameData.Players.Equals(expectedPlayers);
        
        await service.SaveGameStartAsync(gameStartData);
        
        localRepositoryMock.Verify(x => x.AddGameData(It.Is<GameData>(g => gameDataIsAppropriate(g))), Times.Once);
    }

    private static Mock<IHttpGameDataRepository> GetHttpRepositoryMock(int returnedId = 1)
    {
        var mock = new Mock<IHttpGameDataRepository>();
        mock.Setup(x => x.PostGameDataAsync(It.IsAny<GameData>())).ReturnsAsync(new GameData(returnedId, "someconfig", DateTime.Now, new List<int>()));
        return mock;
    }
    
    private static Mock<ICurrentTimeProvider> GetTimeProviderMock()
    {
        var mock = new Mock<ICurrentTimeProvider>();
        mock.Setup(x => x.GetCurrentTime()).Returns(DateTime.Now);
        return mock;
    }
    
    private static Mock<IGameStartDataJsonConverter> GetJsonConverterMock()
    {
        var mock = new Mock<IGameStartDataJsonConverter>();
        mock.Setup(x => x.GetJson(It.IsAny<GameStartData>())).Returns("someconfig");
        return mock;
    }

    private static Mock<ILocalGameDataRepository> GetLocalRepositoryMock()
    {
        return new Mock<ILocalGameDataRepository>();
    }
}