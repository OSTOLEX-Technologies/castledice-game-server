using castledice_game_data_logic;
using castledice_game_data_logic.ConfigsData;
using castledice_game_data_logic.TurnSwitchConditions;
using castledice_game_logic;
using castledice_game_logic.GameConfiguration;
using castledice_game_logic.TurnsLogic.TurnSwitchConditions;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Providers;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameStartDataCreationTests;

public class GameStartDataCreatorTests
{
    [Theory]
    [InlineData("1.0.0")]
    [InlineData("1.0.1")]
    [InlineData("1.1.0")]
    [InlineData("1.1.1")]
    public void CreateGameStartData_ShouldReturnGameStartData_WithVersionFromVersionProvider(string version)
    {
        var versionProviderMock = new Mock<IGameStartDataVersionProvider>();
        versionProviderMock.Setup(x => x.GetGameStartDataVersion()).Returns(version);
        var gameStartDataCreator = new GameStartDataCreatorBuilder
        {
            VersionProvider = versionProviderMock.Object
        };
        
        var gameStartData = gameStartDataCreator.Build().CreateGameStartData(GetGame());
        
        Assert.Equal(version, gameStartData.Version);
    }
    
    [Fact]
    public void CreateGameStartData_ShouldReturnGameStartData_WithBoardDataFromBoardDataProvider()
    {
        var boardDataProviderMock = new Mock<IBoardDataProvider>();
        var expectedBoardData = GetBoardData();
        boardDataProviderMock.Setup(x => x.GetBoardData(It.IsAny<Board>())).Returns(expectedBoardData);
        var gameStartDataCreator = new GameStartDataCreatorBuilder
        {
            BoardDataProvider = boardDataProviderMock.Object
        };
        
        var gameStartData = gameStartDataCreator.Build().CreateGameStartData(GetGame());
        
        Assert.Same(expectedBoardData, gameStartData.BoardData);
    }
    
    [Fact]
    public void CreateGameStartData_ShouldReturnGameStartData_WithPlaceablesConfigFromPlaceablesConfigProvider()
    {
        var provider = new Mock<IPlaceablesConfigDataProvider>();
        var expectedData = new PlaceablesConfigData(new KnightConfigData(1, 2));
        provider.Setup(x => x.GetPlaceablesConfigData(It.IsAny<PlaceablesConfig>())).Returns(expectedData);
        var gameStartDataCreator = new GameStartDataCreatorBuilder
        {
            PlaceablesConfigProvider = provider.Object
        };
        
        var gameStartData = gameStartDataCreator.Build().CreateGameStartData(GetGame());
        
        Assert.Same(expectedData, gameStartData.PlaceablesConfigData);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 1)]
    [InlineData(13, 15)]
    public void CreateGameStartData_ShouldCreateGameStartDataWithPlayersIdsList_FromGivenGame(int firstPlayerId, int secondPlayerId)
    {
        var game = GetGame(GetPlayer(firstPlayerId), GetPlayer(secondPlayerId));
        var playersIds = game.GetAllPlayersIds();
        var gameStartDataCreator = new GameStartDataCreatorBuilder().Build();
        
        var gameStartData = gameStartDataCreator.CreateGameStartData(game);
        var actualPlayersIds = gameStartData.PlayersData.Select(x => x.PlayerId).ToList();
        
        Assert.Equal(playersIds, actualPlayersIds);
    }

    [Fact]
    public void CreateGameStartData_ShouldCreateGameStartData_WithTscConfigDataFromProvider()
    {
        var providerMock = new Mock<ITscConfigDataProvider>();
        var expectedData = new TscConfigData(new List<TscType> { TscType.SwitchByActionPoints });
        providerMock.Setup(x => x.GetTscConfigData(It.IsAny<TurnSwitchConditionsConfig>())).Returns(expectedData);
        var gameStartDataCreator = new GameStartDataCreatorBuilder
        {
            TscConfigProvider = providerMock.Object
        };
        
        var gameStartData = gameStartDataCreator.Build().CreateGameStartData(GetGame());
        
        Assert.Same(expectedData, gameStartData.TscConfigData);
    }

    [Fact]
    public void CreateGameStartData_ShouldCreateGameStartData_WithPlayersDataFromProvider()
    {
        var providerMock = new Mock<IPlayersDataListCreator>();
        var expectedData = new List<PlayerData> { GetPlayerData() };
        var game = GetGame();
        providerMock.Setup(x => x.GetPlayersData(game.GetAllPlayers())).Returns(expectedData);
        var gameStartDataCreator = new GameStartDataCreatorBuilder
        {
            PlayersDataListCreator = providerMock.Object
        };
        
        var gameStartData = gameStartDataCreator.Build().CreateGameStartData(game);
        
        Assert.Same(expectedData, gameStartData.PlayersData);
    }

    private class GameStartDataCreatorBuilder
    {
        public IGameStartDataVersionProvider VersionProvider { get; set; } = new Mock<IGameStartDataVersionProvider>().Object;
        public IBoardDataProvider BoardDataProvider { get; set; } = new Mock<IBoardDataProvider>().Object;
        public IPlaceablesConfigDataProvider PlaceablesConfigProvider { get; set; } = new Mock<IPlaceablesConfigDataProvider>().Object;
        public ITscConfigDataProvider TscConfigProvider { get; set; } = new Mock<ITscConfigDataProvider>().Object;
        public IPlayersDataListCreator PlayersDataListCreator { get; set; } = new Mock<IPlayersDataListCreator>().Object;
        
        public GameStartDataCreator Build()
        {
            return new GameStartDataCreator(VersionProvider, BoardDataProvider, PlaceablesConfigProvider, TscConfigProvider, PlayersDataListCreator);
        }
    }
}