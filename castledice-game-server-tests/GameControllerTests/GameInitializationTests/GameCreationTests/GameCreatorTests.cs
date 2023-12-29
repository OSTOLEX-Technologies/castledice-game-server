using castledice_game_logic;
using castledice_game_logic.GameConfiguration;
using castledice_game_server.GameController.GameInitialization.GameCreation;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.BoardConfigCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.TscConfigCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlaceablesConfigCreators;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests;

public class GameCreatorTests
{
    [Fact]
    public void CreateGame_ShouldReturnGame_FromGivenGameConstructorWrapper()
    {
        var expectedGame = GetGame();
        var wrapperMock = new Mock<IGameConstructorWrapper>();
        wrapperMock.Setup(x => x.ConstructGame(It.IsAny<List<Player>>(), 
                It.IsAny<BoardConfig>(), It.IsAny<PlaceablesConfig>(), It.IsAny<TurnSwitchConditionsConfig>()))
            .Returns(expectedGame);
        var creator = new GameCreatorBuilder
        {
            GameConstructorWrapper = wrapperMock.Object
        }.Build();

        var actualGame = creator.CreateGame(new List<int>());

        Assert.Same(expectedGame, actualGame);
    }

    [Fact]
    public void CreateGame_ShouldCallGetPlayersListOnGivenCreator_OnlyOnce()
    {
        var creatorMock = new Mock<IPlayersListCreator>();
        var creator = new GameCreatorBuilder
        {
            PlayersListCreator = creatorMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        creatorMock.Verify(x => x.GetPlayersList(It.IsAny<List<int>>()), Times.Once);
    }

    [Fact]
    public void CreateGame_ShouldCallGetPlayersListOnGivenCreator_WithGivenIdsList()
    {
        var expectedIdsList = new List<int>();
        var creatorMock = new Mock<IPlayersListCreator>();
        var creator = new GameCreatorBuilder
        {
            PlayersListCreator = creatorMock.Object
        }.Build();
        
        creator.CreateGame(expectedIdsList);
        
        creatorMock.Verify(x => x.GetPlayersList(expectedIdsList), Times.Once);
    }

    [Fact]
    public void CreateGame_ShouldPassPlayersListFromCreator_IntoGivenConstructorWrapper()
    {
        var expectedPlayersList = new List<Player>();
        var playersListCreatorMock = new Mock<IPlayersListCreator>();
        playersListCreatorMock.Setup(x => x.GetPlayersList(It.IsAny<List<int>>())).Returns(expectedPlayersList);
        var wrapperMock = GetGameConstructorWrapperMock();
        var creator = new GameCreatorBuilder
        {
            PlayersListCreator = playersListCreatorMock.Object,
            GameConstructorWrapper = wrapperMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        wrapperMock.Verify(x => x.ConstructGame(expectedPlayersList, It.IsAny<BoardConfig>(), It.IsAny<PlaceablesConfig>(), It.IsAny<TurnSwitchConditionsConfig>()), Times.Once);
    }
    
    [Fact]
    public void CreateGame_ShouldCallGetBoardConfigOnGivenCreator_OnlyOnce()
    {
        var creatorMock = new Mock<IBoardConfigCreator>();
        var creator = new GameCreatorBuilder
        {
            BoardConfigCreator = creatorMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        creatorMock.Verify(x => x.GetBoardConfig(It.IsAny<List<Player>>()), Times.Once);
    }

    [Fact]
    public void CreateGame_ShouldPassPlayersListFromPlayersListCreator_ToGivenBoardConfigCreator()
    {
        var playersList = new List<Player>();
        var playersListCreatorMock = new Mock<IPlayersListCreator>();
        playersListCreatorMock.Setup(x => x.GetPlayersList(It.IsAny<List<int>>())).Returns(playersList);
        var boardConfigCreatorMock = new Mock<IBoardConfigCreator>();
        var creator = new GameCreatorBuilder
        {
            PlayersListCreator = playersListCreatorMock.Object,
            BoardConfigCreator = boardConfigCreatorMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        boardConfigCreatorMock.Verify(x => x.GetBoardConfig(playersList), Times.Once);
    }
    
    [Fact]
    public void CreateGame_ShouldPassBoardConfigFromCreator_IntoGivenConstructorWrapper()
    {
        var expectedBoardConfig = GetBoardConfig();
        var boardConfigCreatorMock = new Mock<IBoardConfigCreator>();
        boardConfigCreatorMock.Setup(x => x.GetBoardConfig(It.IsAny<List<Player>>())).Returns(expectedBoardConfig);
        var wrapperMock = GetGameConstructorWrapperMock();
        var creator = new GameCreatorBuilder
        {
            BoardConfigCreator = boardConfigCreatorMock.Object,
            GameConstructorWrapper = wrapperMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        wrapperMock.Verify(x => x.ConstructGame(It.IsAny<List<Player>>(), expectedBoardConfig, It.IsAny<PlaceablesConfig>(), It.IsAny<TurnSwitchConditionsConfig>()), Times.Once);
    }
    
    [Fact]
    public void CreateGame_ShouldCallGetPlaceablesConfigOnGivenCreator_OnlyOnce()
    {
        var creatorMock = new Mock<IPlaceablesConfigCreator>();
        var creator = new GameCreatorBuilder
        {
            PlaceablesConfigCreator = creatorMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        creatorMock.Verify(x => x.GetPlaceablesConfig(), Times.Once);
    }
    
    [Fact]
    public void CreateGame_ShouldPassPlaceablesConfigFromCreator_IntoGivenConstructorWrapper()
    {
        var expectedPlaceablesConfig = GetPlaceablesConfig();
        var placeablesConfigCreatorMock = new Mock<IPlaceablesConfigCreator>();
        placeablesConfigCreatorMock.Setup(x => x.GetPlaceablesConfig()).Returns(expectedPlaceablesConfig);
        var wrapperMock = GetGameConstructorWrapperMock();
        var creator = new GameCreatorBuilder
        {
            PlaceablesConfigCreator = placeablesConfigCreatorMock.Object,
            GameConstructorWrapper = wrapperMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        wrapperMock.Verify(x => x.ConstructGame(It.IsAny<List<Player>>(), It.IsAny<BoardConfig>(), expectedPlaceablesConfig, It.IsAny<TurnSwitchConditionsConfig>()), Times.Once);
    }

    [Fact]
    public void CreateGame_ShouldPassTscConfigFromCreator_ToGameConstructorWrapper()
    {
        var expectedTscConfig = GetTurnSwitchConditionsConfig();
        var tscConfigCreatorMock = new Mock<ITscConfigCreator>();
        tscConfigCreatorMock.Setup(x => x.GetTurnSwitchConditionsConfig()).Returns(expectedTscConfig);
        var gameConstructorWrapperMock = new Mock<IGameConstructorWrapper>();
        var creator = new GameCreatorBuilder
        {
            TscListCreator = tscConfigCreatorMock.Object,
            GameConstructorWrapper = gameConstructorWrapperMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        gameConstructorWrapperMock.Verify(x => x.ConstructGame(It.IsAny<List<Player>>(), It.IsAny<BoardConfig>(), It.IsAny<PlaceablesConfig>(), expectedTscConfig), Times.Once);
    }

    private class GameCreatorBuilder
    {
        public IPlayersListCreator PlayersListCreator { get; set; } = new Mock<IPlayersListCreator>().Object;
        public IBoardConfigCreator BoardConfigCreator { get; set; } = new Mock<IBoardConfigCreator>().Object;
        public IPlaceablesConfigCreator PlaceablesConfigCreator { get; set; } = new Mock<IPlaceablesConfigCreator>().Object;
        public ITscConfigCreator TscListCreator { get; set; } = GetTscConfigCreatorMock().Object;
        public IGameConstructorWrapper GameConstructorWrapper { get; set; } = GetGameConstructorWrapperMock().Object;
        
        public GameCreator Build()
        {
            return new GameCreator(PlayersListCreator, BoardConfigCreator, PlaceablesConfigCreator, TscListCreator, GameConstructorWrapper);
        }
    }
}