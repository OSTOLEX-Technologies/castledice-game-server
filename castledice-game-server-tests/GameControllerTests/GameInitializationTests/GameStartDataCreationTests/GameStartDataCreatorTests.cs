using castledice_game_data_logic;
using castledice_game_data_logic.Content.Placeable;
using castledice_game_logic;
using castledice_game_logic.GameConfiguration;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlaceablesConfigProviders;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Providers;
using static castledice_game_server_tests.ObjectCreationUtility;
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
    
    [Fact]
    public void CreateGameStartData_ShouldReturnGameStartData_WithDecksDataFromDecksDataProvider()
    {
        var provider = new Mock<IDecksDataProvider>();
        var expectedData = new List<PlayerDeckData>();
        provider.Setup(x => x.GetPlayersDecksData(It.IsAny<Game>())).Returns(expectedData);
        var gameStartDataCreator = new GameStartDataCreatorBuilder
        {
            DecksDataProvider = provider.Object
        };
        
        var gameStartData = gameStartDataCreator.Build().CreateGameStartData(GetGame());
        
        Assert.Same(expectedData, gameStartData.Decks);
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
        
        Assert.Equal(playersIds, gameStartData.PlayersIds);
    }

    private class GameStartDataCreatorBuilder
    {
        public IGameStartDataVersionProvider VersionProvider { get; set; } = new Mock<IGameStartDataVersionProvider>().Object;
        public IBoardDataProvider BoardDataProvider { get; set; } = new Mock<IBoardDataProvider>().Object;
        public IPlaceablesConfigDataProvider PlaceablesConfigProvider { get; set; } = new Mock<IPlaceablesConfigDataProvider>().Object;
        public IDecksDataProvider DecksDataProvider { get; set; } = new Mock<IDecksDataProvider>().Object;
        
        public GameStartDataCreator Build()
        {
            return new GameStartDataCreator(VersionProvider, BoardDataProvider, PlaceablesConfigProvider, DecksDataProvider);
        }
    }
}