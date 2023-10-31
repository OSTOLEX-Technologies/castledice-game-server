using System.Globalization;
using castledice_game_data_logic;
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
    public async void SaveGameStartAsync_ShouldSendGameData_WithConfigFromGameStartDataJsonConverter(
        string expectedConfig)
    {
        var dataSenderMock = GetDataSenderMock();
        var jsonConverterMock = GetJsonConverterMock();
        jsonConverterMock.Setup(x => x.GetJson(It.IsAny<GameStartData>())).Returns(expectedConfig);
        var service = new HttpGameSavingService(dataSenderMock.Object, GetTimeProviderMock().Object,
            jsonConverterMock.Object);
        Predicate<GameData> gameDataHasNeededString = gameData => gameData.Config == expectedConfig;

        await service.SaveGameStartAsync(GetGameStartData());

        dataSenderMock.Verify(x => x.SendGameDataAsync(It.Is<GameData>(g => gameDataHasNeededString(g)), It.IsAny<HttpMethod>()), Times.Once);
    }

    [Theory]
    [InlineData("2/27/2023 2:06:49")]
    [InlineData("2/15/2022 2:06:50")]
    [InlineData("3/29/2023 2:06:51")]
    public async void SaveGameStartAsync_ShouldSendGameData_WithGameStartedTimeFromGivenCurrentTimeProvider(
        string expectedTimeStr)
    {
        var expectedTime = DateTime.Parse(expectedTimeStr, CultureInfo.InvariantCulture);
        var dataSenderMock = GetDataSenderMock();
        var timeProviderMock = GetTimeProviderMock();
        timeProviderMock.Setup(x => x.GetCurrentTime()).Returns(expectedTime);
        var service = new HttpGameSavingService(dataSenderMock.Object, timeProviderMock.Object,
            GetJsonConverterMock().Object);
        Predicate<GameData> gameDataHasNeededTime = gameData => gameData.GameStartedTime == expectedTime;

        await service.SaveGameStartAsync(GetGameStartData());

        dataSenderMock.Verify(x => x.SendGameDataAsync(It.Is<GameData>(g => gameDataHasNeededTime(g)), It.IsAny<HttpMethod>()), Times.Once);
    }

    [Fact]
    public async void SaveGameStartAsync_ShouldSendGameData_WithPlayersListFromGivenGameStartData()
    {
        var gameStartData = GetGameStartData();
        var dataSenderMock = GetDataSenderMock();
        var service = new HttpGameSavingService(dataSenderMock.Object, GetTimeProviderMock().Object,
            GetJsonConverterMock().Object);
        Predicate<GameData> gameDataHasNeededPlayersList = gameData => gameData.Players.Equals(gameStartData.PlayersIds);
        
        await service.SaveGameStartAsync(gameStartData);

        dataSenderMock.Verify(x => x.SendGameDataAsync(It.Is<GameData>(g => gameDataHasNeededPlayersList(g)), It.IsAny<HttpMethod>()), Times.Once);
    }

    [Fact]
    public async void SaveGameStartAsync_ShouldSendGameData_WithGetHttpMethod()
    {
        var dataSenderMock = GetDataSenderMock();
        var service = new HttpGameSavingService(dataSenderMock.Object, GetTimeProviderMock().Object,
            GetJsonConverterMock().Object);
        
        await service.SaveGameStartAsync(GetGameStartData());
        
        dataSenderMock.Verify(x => x.SendGameDataAsync(It.IsAny<GameData>(), HttpMethod.Get), Times.Once);
    }

private static Mock<IHttpGameDataSender> GetDataSenderMock(int returnedId = 1)
    {
        var mock = new Mock<IHttpGameDataSender>();
        mock.Setup(x => x.SendGameDataAsync(It.IsAny<GameData>(), It.IsAny<HttpMethod>())).ReturnsAsync(new GameData(returnedId, "someconfig", DateTime.Now, new List<int>()));
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
}