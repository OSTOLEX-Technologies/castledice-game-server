using castledice_game_logic;
using castledice_game_logic.GameConfiguration;
using castledice_game_logic.GameObjects;
using castledice_game_server.GameController.GameInitialization.GameCreation;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders;
using Moq;
using static castledice_game_server_tests.ObjectCreationUtility;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests;

public class GameCreatorTests
{
    [Fact]
    public void CreateGame_ShouldReturnGame_FromGivenGameConstructorWrapper()
    {
        var expectedGame = GetGame();
        var wrapperMock = new Mock<IGameConstructorWrapper>();
        wrapperMock.Setup(x => x.ConstructGame(It.IsAny<List<Player>>(), It.IsAny<BoardConfig>(), It.IsAny<PlaceablesConfig>(), It.IsAny<IDecksList>()))
            .Returns(expectedGame);
        var creator = new GameCreatorBuilder
        {
            GameConstructorWrapper = wrapperMock.Object
        }.Build();

        var actualGame = creator.CreateGame(new List<int>());

        Assert.Same(expectedGame, actualGame);
    }

    [Fact]
    public void CreateGame_ShouldCallGetPlayersListOnGivenProvider_OnlyOnce()
    {
        var providerMock = new Mock<IPlayersListProvider>();
        var creator = new GameCreatorBuilder
        {
            PlayersListProvider = providerMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        providerMock.Verify(x => x.GetPlayersList(It.IsAny<List<int>>()), Times.Once);
    }

    [Fact]
    public void CreateGame_ShouldCallGetPlayersListOnGivenProvider_WithGivenIdsList()
    {
        var expectedIdsList = new List<int>();
        var providerMock = new Mock<IPlayersListProvider>();
        var creator = new GameCreatorBuilder
        {
            PlayersListProvider = providerMock.Object
        }.Build();
        
        creator.CreateGame(expectedIdsList);
        
        providerMock.Verify(x => x.GetPlayersList(expectedIdsList), Times.Once);
    }

    [Fact]
    public void CreateGame_ShouldPassPlayersListFromProvider_IntoGivenConstructorWrapper()
    {
        var expectedPlayersList = new List<Player>();
        var playersListProviderMock = new Mock<IPlayersListProvider>();
        playersListProviderMock.Setup(x => x.GetPlayersList(It.IsAny<List<int>>())).Returns(expectedPlayersList);
        var wrapperMock = new Mock<IGameConstructorWrapper>();
        var creator = new GameCreatorBuilder
        {
            PlayersListProvider = playersListProviderMock.Object,
            GameConstructorWrapper = wrapperMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        wrapperMock.Verify(x => x.ConstructGame(expectedPlayersList, It.IsAny<BoardConfig>(), It.IsAny<PlaceablesConfig>(), It.IsAny<IDecksList>()), Times.Once);
    }
    
    [Fact]
    public void CreateGame_ShouldCallGetBoardConfigOnGivenProvider_OnlyOnce()
    {
        var providerMock = new Mock<IBoardConfigProvider>();
        var creator = new GameCreatorBuilder
        {
            BoardConfigProvider = providerMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        providerMock.Verify(x => x.GetBoardConfig(), Times.Once);
    }
    
    [Fact]
    public void CreateGame_ShouldPassBoardConfigFromProvider_IntoGivenConstructorWrapper()
    {
        var expectedBoardConfig = GetBoardConfig();
        var boardConfigProviderMock = new Mock<IBoardConfigProvider>();
        boardConfigProviderMock.Setup(x => x.GetBoardConfig()).Returns(expectedBoardConfig);
        var wrapperMock = new Mock<IGameConstructorWrapper>();
        var creator = new GameCreatorBuilder
        {
            BoardConfigProvider = boardConfigProviderMock.Object,
            GameConstructorWrapper = wrapperMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        wrapperMock.Verify(x => x.ConstructGame(It.IsAny<List<Player>>(), expectedBoardConfig, It.IsAny<PlaceablesConfig>(), It.IsAny<IDecksList>()), Times.Once);
    }
    
    [Fact]
    public void CreateGame_ShouldCallGetPlaceablesConfigOnGivenProvider_OnlyOnce()
    {
        var providerMock = new Mock<IPlaceablesConfigProvider>();
        var creator = new GameCreatorBuilder
        {
            PlaceablesConfigProvider = providerMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        providerMock.Verify(x => x.GetPlaceablesConfig(), Times.Once);
    }
    
    [Fact]
    public void CreateGame_ShouldPassPlaceablesConfigFromProvider_IntoGivenConstructorWrapper()
    {
        var expectedPlaceablesConfig = GetPlaceablesConfig();
        var placeablesConfigProviderMock = new Mock<IPlaceablesConfigProvider>();
        placeablesConfigProviderMock.Setup(x => x.GetPlaceablesConfig()).Returns(expectedPlaceablesConfig);
        var wrapperMock = new Mock<IGameConstructorWrapper>();
        var creator = new GameCreatorBuilder
        {
            PlaceablesConfigProvider = placeablesConfigProviderMock.Object,
            GameConstructorWrapper = wrapperMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        wrapperMock.Verify(x => x.ConstructGame(It.IsAny<List<Player>>(), It.IsAny<BoardConfig>(), expectedPlaceablesConfig, It.IsAny<IDecksList>()), Times.Once);
    }
    
    [Fact]
    public void CreateGame_ShouldCallGetPlayersDecksOnGivenProvider_OnlyOnce()
    {
        var providerMock = new Mock<IPlayersDecksProvider>();
        var creator = new GameCreatorBuilder
        {
            PlayersDecksProvider = providerMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        providerMock.Verify(x => x.GetPlayersDecksList(It.IsAny<List<Player>>()), Times.Once);
    }

    [Fact]
    public void CreateGame_ShouldPassGivenIdsList_ToGivenPlayerDecksProvider()
    {
        var expectedIdsList = new List<int>();
        var playersDecksProviderMock = new Mock<IPlayersDecksProvider>();
        var creator = new GameCreatorBuilder
        {
            PlayersDecksProvider = playersDecksProviderMock.Object
        }.Build();
        
        creator.CreateGame(expectedIdsList);
        
        playersDecksProviderMock.Verify(x => x.GetPlayersDecksList(It.IsAny<List<Player>>()), Times.Once);
    }
    
    [Fact]
    public void CreateGame_ShouldPassPlayersDecksFromProvider_IntoGivenConstructorWrapper()
    {
        var expectedPlayersDecks = new Mock<IDecksList>().Object;
        var playersDecksProviderMock = new Mock<IPlayersDecksProvider>();
        playersDecksProviderMock.Setup(x => x.GetPlayersDecksList(It.IsAny<List<Player>>())).Returns(expectedPlayersDecks);
        var wrapperMock = new Mock<IGameConstructorWrapper>();
        var creator = new GameCreatorBuilder
        {
            PlayersDecksProvider = playersDecksProviderMock.Object,
            GameConstructorWrapper = wrapperMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        wrapperMock.Verify(x => x.ConstructGame(It.IsAny<List<Player>>(), It.IsAny<BoardConfig>(), It.IsAny<PlaceablesConfig>(), expectedPlayersDecks), Times.Once);
    }

    private class GameCreatorBuilder
    {
        public IPlayersListProvider PlayersListProvider { get; set; } = new Mock<IPlayersListProvider>().Object;
        public IBoardConfigProvider BoardConfigProvider { get; set; } = new Mock<IBoardConfigProvider>().Object;
        public IPlaceablesConfigProvider PlaceablesConfigProvider { get; set; } = new Mock<IPlaceablesConfigProvider>().Object;
        public IPlayersDecksProvider PlayersDecksProvider { get; set; } = new Mock<IPlayersDecksProvider>().Object;
        public IGameConstructorWrapper GameConstructorWrapper { get; set; } = new Mock<IGameConstructorWrapper>().Object;
        
        public GameCreator Build()
        {
            return new GameCreator(PlayersListProvider, BoardConfigProvider, PlaceablesConfigProvider, PlayersDecksProvider, GameConstructorWrapper);
        }
    }
    
}