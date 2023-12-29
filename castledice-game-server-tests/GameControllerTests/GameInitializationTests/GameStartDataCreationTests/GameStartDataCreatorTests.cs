using castledice_game_data_logic;
using castledice_game_data_logic.ConfigsData;
using castledice_game_data_logic.TurnSwitchConditions;
using castledice_game_logic;
using castledice_game_logic.GameConfiguration;
using castledice_game_logic.TurnsLogic.TurnSwitchConditions;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators;
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
    public void CreateGameStartData_ShouldReturnGameStartData_WithVersionFromVersionCreator(string version)
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
    public void CreateGameStartData_ShouldReturnGameStartData_WithBoardDataFromBoardDataCreator()
    {
        var boardDataCreatorMock = new Mock<IBoardDataCreator>();
        var expectedBoardData = GetBoardData();
        boardDataCreatorMock.Setup(x => x.GetBoardData(It.IsAny<Board>())).Returns(expectedBoardData);
        var gameStartDataCreator = new GameStartDataCreatorBuilder
        {
            BoardDataCreator = boardDataCreatorMock.Object
        };
        
        var gameStartData = gameStartDataCreator.Build().CreateGameStartData(GetGame());
        
        Assert.Same(expectedBoardData, gameStartData.BoardData);
    }
    
    [Fact]
    public void CreateGameStartData_ShouldReturnGameStartData_WithPlaceablesConfigFromPlaceablesConfigCreator()
    {
        var creator = new Mock<IPlaceablesConfigDataCreator>();
        var expectedData = new PlaceablesConfigData(new KnightConfigData(1, 2));
        creator.Setup(x => x.GetPlaceablesConfigData(It.IsAny<PlaceablesConfig>())).Returns(expectedData);
        var gameStartDataCreator = new GameStartDataCreatorBuilder
        {
            PlaceablesConfigCreator = creator.Object
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
    public void CreateGameStartData_ShouldCreateGameStartData_WithTscConfigDataFromCreator()
    {
        var creatorMock = new Mock<ITscConfigDataCreator>();
        var expectedData = new TscConfigData(new List<TscType> { TscType.SwitchByActionPoints });
        creatorMock.Setup(x => x.GetTscConfigData(It.IsAny<TurnSwitchConditionsConfig>())).Returns(expectedData);
        var gameStartDataCreator = new GameStartDataCreatorBuilder
        {
            TscConfigCreator = creatorMock.Object
        };
        
        var gameStartData = gameStartDataCreator.Build().CreateGameStartData(GetGame());
        
        Assert.Same(expectedData, gameStartData.TscConfigData);
    }

    [Fact]
    public void CreateGameStartData_ShouldCreateGameStartData_WithPlayersDataFromCreator()
    {
        var creatorMock = new Mock<IPlayersDataListCreator>();
        var expectedData = new List<PlayerData> { GetPlayerData() };
        var game = GetGame();
        creatorMock.Setup(x => x.GetPlayersData(game.GetAllPlayers())).Returns(expectedData);
        var gameStartDataCreator = new GameStartDataCreatorBuilder
        {
            PlayersDataListCreator = creatorMock.Object
        };
        
        var gameStartData = gameStartDataCreator.Build().CreateGameStartData(game);
        
        Assert.Same(expectedData, gameStartData.PlayersData);
    }

    private class GameStartDataCreatorBuilder
    {
        public IGameStartDataVersionProvider VersionProvider { get; set; } = new Mock<IGameStartDataVersionProvider>().Object;
        public IBoardDataCreator BoardDataCreator { get; set; } = new Mock<IBoardDataCreator>().Object;
        public IPlaceablesConfigDataCreator PlaceablesConfigCreator { get; set; } = new Mock<IPlaceablesConfigDataCreator>().Object;
        public ITscConfigDataCreator TscConfigCreator { get; set; } = new Mock<ITscConfigDataCreator>().Object;
        public IPlayersDataListCreator PlayersDataListCreator { get; set; } = new Mock<IPlayersDataListCreator>().Object;
        
        public GameStartDataCreator Build()
        {
            return new GameStartDataCreator(VersionProvider, BoardDataCreator, PlaceablesConfigCreator, TscConfigCreator, PlayersDataListCreator);
        }
    }
}