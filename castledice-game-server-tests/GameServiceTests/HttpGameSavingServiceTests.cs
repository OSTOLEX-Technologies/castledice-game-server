using System.Globalization;
using System.Net;
using castledice_game_data_logic;
using castledice_game_server.Exceptions;
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
    public async void SaveGameStartAsync_ShouldWrapHttpRequestExceptionIntoGameNotSavedException_IfCaughtOne()
    {
        var exception = new HttpRequestException();
        var dataSenderMock = new Mock<IHttpGameDataRepository>();
        dataSenderMock.Setup(x => x.PostGameDataAsync(It.IsAny<GameData>())).ThrowsAsync(exception);
        var service = new HttpGameSavingService(dataSenderMock.Object, GetTimeProviderMock().Object,
            GetJsonConverterMock().Object, GetLocalRepositoryMock().Object);
        
        var result = await Record.ExceptionAsync(() => service.SaveGameStartAsync(GetGameStartData()));
        
        Assert.IsType<GameNotSavedException>(result);
        Assert.Same(exception, result.InnerException);
    }

    [Fact]
    public async void SaveGameStartAsync_ShouldAddGameDataFromResponse_ToLocalRepository()
    {
        var gameData = new GameData(1, "aaa", DateTime.Now, new List<int>());
        var dataSenderMock = GetHttpRepositoryMock();
        dataSenderMock.Setup(x => x.PostGameDataAsync(It.IsAny<GameData>())).ReturnsAsync(gameData);
        var localRepositoryMock = GetLocalRepositoryMock();
        var service = new HttpGameSavingService(dataSenderMock.Object, GetTimeProviderMock().Object,
            GetJsonConverterMock().Object, localRepositoryMock.Object);
        
        await service.SaveGameStartAsync(GetGameStartData());
        
        localRepositoryMock.Verify(x => x.AddGameData(gameData), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async void SaveGameEndAsync_ShouldRetrieveGameDataFromHttpRepository_WithGetRequest(int gameId)
    {
        var gameData = new GameData(gameId, "aaa", DateTime.Now, new List<int>());
        var dataSenderMock = GetHttpRepositoryMock();
        dataSenderMock.Setup(x => x.GetGameDataAsync(gameId)).ReturnsAsync(gameData);
        var service = new HttpGameSavingService(dataSenderMock.Object, GetTimeProviderMock().Object,
            GetJsonConverterMock().Object, GetLocalRepositoryMock().Object);
        
        await service.SaveGameEndAsync(gameId, 0, "history");
        
        dataSenderMock.Verify(x => x.GetGameDataAsync(gameId), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async void SaveGameEndAsync_ShouldRetrieveGameDataFromLocalRepository_IfHttpRepositoryThrewAndHttpException(
        int gameId)
    {
        var gameData = new GameData(gameId, "aaa", DateTime.Now, new List<int>());
        var dataSenderMock = GetHttpRepositoryMock();
        dataSenderMock.Setup(x => x.GetGameDataAsync(gameId)).ThrowsAsync(new HttpRequestException());
        var localRepositoryMock = GetLocalRepositoryMock();
        localRepositoryMock.Setup(x => x.GetGameData(gameId)).Returns(gameData);
        var service = new HttpGameSavingService(dataSenderMock.Object, GetTimeProviderMock().Object,
            GetJsonConverterMock().Object, localRepositoryMock.Object);
        
        await service.SaveGameEndAsync(gameId, 0, "history");
        
        localRepositoryMock.Verify(x => x.GetGameData(gameId), Times.Once);
    }

    [Fact]
    public async void SaveGameEndAsync_ShouldWrappGameDataNotFoundExceptionFromLocalRepository_ToGameNotSavedException()
    {
        var exception = new GameDataNotFoundException();
        var dataSenderMock = GetHttpRepositoryMock();
        dataSenderMock.Setup(x => x.GetGameDataAsync(It.IsAny<int>())).ThrowsAsync(new HttpRequestException());
        var localRepositoryMock = GetLocalRepositoryMock();
        localRepositoryMock.Setup(x => x.GetGameData(It.IsAny<int>())).Throws(exception);
        var service = new HttpGameSavingService(dataSenderMock.Object, GetTimeProviderMock().Object,
            GetJsonConverterMock().Object, localRepositoryMock.Object);
        
        var result = await Record.ExceptionAsync(() => service.SaveGameEndAsync(1, 0, "history"));
        
        Assert.IsType<GameNotSavedException>(result);
        Assert.Same(exception, result.InnerException);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async void SaveGameEndAsync_ShouldPutGameDataToHttpRepository_WithGivenWinnerId(int winnerId)
    {
        var gameData = new GameData(1, "aaa", DateTime.Now, new List<int>());
        var dataSenderMock = GetHttpRepositoryMock();
        dataSenderMock.Setup(x => x.GetGameDataAsync(It.IsAny<int>())).ReturnsAsync(gameData);
        var service = new HttpGameSavingService(dataSenderMock.Object, GetTimeProviderMock().Object,
            GetJsonConverterMock().Object, GetLocalRepositoryMock().Object);
        
        await service.SaveGameEndAsync(1, winnerId, "history");
        
        dataSenderMock.Verify(x => x.PutGameDataAsync(It.Is<GameData>(g => g.WinnerId == winnerId)), Times.Once);
    }

    [Theory]
    [InlineData("somehistory")]
    [InlineData("interestinghistory")]
    [InlineData("someotherhistory")]
    public async void SaveGameEndAsync_ShouldPutGameDataToHttpRepository_WithGivenHistory(string history)
    {
        var gameData = new GameData(1, "aaa", DateTime.Now, new List<int>());
        var dataSenderMock = GetHttpRepositoryMock();
        dataSenderMock.Setup(x => x.GetGameDataAsync(It.IsAny<int>())).ReturnsAsync(gameData);
        var service = new HttpGameSavingService(dataSenderMock.Object, GetTimeProviderMock().Object,
            GetJsonConverterMock().Object, GetLocalRepositoryMock().Object);
        
        await service.SaveGameEndAsync(1, 0, history);
        
        dataSenderMock.Verify(x => x.PutGameDataAsync(It.Is<GameData>(g => g.History == history)), Times.Once);
    }

    [Theory]
    [InlineData("2/27/2023 2:06:49")]
    [InlineData("2/15/2022 2:06:50")]
    [InlineData("3/29/2023 2:06:51")]
    public async void SaveGameEndAsync_ShouldPutGameDatToHttpRepository_WithGameEndTimeFromGivenProvider(string timeStr)
    {
        var expectedTime = DateTime.Parse(timeStr, CultureInfo.InvariantCulture);
        var gameData = new GameData(1, "aaa", DateTime.Now, new List<int>());
        var dataSenderMock = GetHttpRepositoryMock();
        dataSenderMock.Setup(x => x.GetGameDataAsync(It.IsAny<int>())).ReturnsAsync(gameData);
        var timeProviderMock = GetTimeProviderMock();
        timeProviderMock.Setup(x => x.GetCurrentTime()).Returns(expectedTime);
        var service = new HttpGameSavingService(dataSenderMock.Object, timeProviderMock.Object,
            GetJsonConverterMock().Object, GetLocalRepositoryMock().Object);
        
        await service.SaveGameEndAsync(1, 0, "history");
        
        dataSenderMock.Verify(x => x.PutGameDataAsync(It.Is<GameData>(g => g.GameEndedTime == expectedTime)), Times.Once);
    }

    [Fact]
    public async void SaveGameEndAsync_ShouldWrapHttpExceptionFromPutRequest_IntoGameNotSavedException()
    {
        var exception = new HttpRequestException();
        var dataSenderMock = GetHttpRepositoryMock();
        dataSenderMock.Setup(x => x.GetGameDataAsync(It.IsAny<int>()))
            .ReturnsAsync(new GameData(0, "a", DateTime.Now, new List<int>()));
        dataSenderMock.Setup(x => x.PutGameDataAsync(It.IsAny<GameData>())).ThrowsAsync(exception);
        var service = new HttpGameSavingService(dataSenderMock.Object, GetTimeProviderMock().Object,
            GetJsonConverterMock().Object, GetLocalRepositoryMock().Object);
        
        var result = await Record.ExceptionAsync(() => service.SaveGameEndAsync(1, 0, "history"));
        
        Assert.IsType<GameNotSavedException>(result);
        Assert.Same(exception, result.InnerException);
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