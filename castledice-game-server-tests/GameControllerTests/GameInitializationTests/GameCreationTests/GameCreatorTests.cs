﻿using castledice_game_logic;
using castledice_game_logic.GameConfiguration;
using castledice_game_logic.GameObjects;
using castledice_game_server.GameController.GameInitialization.GameCreation;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlaceablesConfigProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersDecksListsProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersListProviders;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.TscConfigProviders;
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
                It.IsAny<BoardConfig>(), It.IsAny<PlaceablesConfig>(), It.IsAny<IDecksList>(), It.IsAny<TurnSwitchConditionsConfig>()))
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
        var wrapperMock = GetGameConstructorWrapperMock();
        var creator = new GameCreatorBuilder
        {
            PlayersListProvider = playersListProviderMock.Object,
            GameConstructorWrapper = wrapperMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        wrapperMock.Verify(x => x.ConstructGame(expectedPlayersList, It.IsAny<BoardConfig>(), It.IsAny<PlaceablesConfig>(), It.IsAny<IDecksList>(), It.IsAny<TurnSwitchConditionsConfig>()), Times.Once);
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
        
        providerMock.Verify(x => x.GetBoardConfig(It.IsAny<List<Player>>()), Times.Once);
    }

    [Fact]
    public void CreateGame_ShouldPassPlayersListFromPlayersListProvider_ToGivenBoardConfigProvider()
    {
        var playersList = new List<Player>();
        var playersListProviderMock = new Mock<IPlayersListProvider>();
        playersListProviderMock.Setup(x => x.GetPlayersList(It.IsAny<List<int>>())).Returns(playersList);
        var boardConfigProviderMock = new Mock<IBoardConfigProvider>();
        var creator = new GameCreatorBuilder
        {
            PlayersListProvider = playersListProviderMock.Object,
            BoardConfigProvider = boardConfigProviderMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        boardConfigProviderMock.Verify(x => x.GetBoardConfig(playersList), Times.Once);
    }
    
    [Fact]
    public void CreateGame_ShouldPassBoardConfigFromProvider_IntoGivenConstructorWrapper()
    {
        var expectedBoardConfig = GetBoardConfig();
        var boardConfigProviderMock = new Mock<IBoardConfigProvider>();
        boardConfigProviderMock.Setup(x => x.GetBoardConfig(It.IsAny<List<Player>>())).Returns(expectedBoardConfig);
        var wrapperMock = GetGameConstructorWrapperMock();
        var creator = new GameCreatorBuilder
        {
            BoardConfigProvider = boardConfigProviderMock.Object,
            GameConstructorWrapper = wrapperMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        wrapperMock.Verify(x => x.ConstructGame(It.IsAny<List<Player>>(), expectedBoardConfig, It.IsAny<PlaceablesConfig>(), It.IsAny<IDecksList>(), It.IsAny<TurnSwitchConditionsConfig>()), Times.Once);
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
        var wrapperMock = GetGameConstructorWrapperMock();
        var creator = new GameCreatorBuilder
        {
            PlaceablesConfigProvider = placeablesConfigProviderMock.Object,
            GameConstructorWrapper = wrapperMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        wrapperMock.Verify(x => x.ConstructGame(It.IsAny<List<Player>>(), It.IsAny<BoardConfig>(), expectedPlaceablesConfig, It.IsAny<IDecksList>(), It.IsAny<TurnSwitchConditionsConfig>()), Times.Once);
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
        
        providerMock.Verify(x => x.GetPlayersDecksList(It.IsAny<List<int>>()), Times.Once);
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
        
        playersDecksProviderMock.Verify(x => x.GetPlayersDecksList(expectedIdsList), Times.Once);
    }
    
    [Fact]
    public void CreateGame_ShouldPassPlayersDecksFromProvider_IntoGivenConstructorWrapper()
    {
        var expectedPlayersDecks = new Mock<IDecksList>().Object;
        var playersDecksProviderMock = new Mock<IPlayersDecksProvider>();
        playersDecksProviderMock.Setup(x => x.GetPlayersDecksList(It.IsAny<List<int>>())).Returns(expectedPlayersDecks);
        var wrapperMock = GetGameConstructorWrapperMock();
        var creator = new GameCreatorBuilder
        {
            PlayersDecksProvider = playersDecksProviderMock.Object,
            GameConstructorWrapper = wrapperMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        wrapperMock.Verify(x => x.ConstructGame(It.IsAny<List<Player>>(), It.IsAny<BoardConfig>(), It.IsAny<PlaceablesConfig>(), expectedPlayersDecks, It.IsAny<TurnSwitchConditionsConfig>()), Times.Once);
    }

    [Fact]
    public void CreateGame_ShouldPassTscConfigFromProvider_ToGameConstructorWrapper()
    {
        var expectedTscConfig = GetTurnSwitchConditionsConfig();
        var tscConfigProviderMock = new Mock<ITscConfigProvider>();
        tscConfigProviderMock.Setup(x => x.GetTurnSwitchConditionsConfig()).Returns(expectedTscConfig);
        var gameConstructorWrapperMock = new Mock<IGameConstructorWrapper>();
        var creator = new GameCreatorBuilder
        {
            TscListProvider = tscConfigProviderMock.Object,
            GameConstructorWrapper = gameConstructorWrapperMock.Object
        }.Build();
        
        creator.CreateGame(new List<int>());
        
        gameConstructorWrapperMock.Verify(x => x.ConstructGame(It.IsAny<List<Player>>(), It.IsAny<BoardConfig>(), It.IsAny<PlaceablesConfig>(), It.IsAny<IDecksList>(), expectedTscConfig), Times.Once);
    }

    private class GameCreatorBuilder
    {
        public IPlayersListProvider PlayersListProvider { get; set; } = new Mock<IPlayersListProvider>().Object;
        public IBoardConfigProvider BoardConfigProvider { get; set; } = new Mock<IBoardConfigProvider>().Object;
        public IPlaceablesConfigProvider PlaceablesConfigProvider { get; set; } = new Mock<IPlaceablesConfigProvider>().Object;
        public ITscConfigProvider TscListProvider { get; set; } = GetTscConfigProviderMock().Object;
        public IPlayersDecksProvider PlayersDecksProvider { get; set; } = new Mock<IPlayersDecksProvider>().Object;
        public IGameConstructorWrapper GameConstructorWrapper { get; set; } = GetGameConstructorWrapperMock().Object;
        
        public GameCreator Build()
        {
            return new GameCreator(PlayersListProvider, BoardConfigProvider, PlaceablesConfigProvider, PlayersDecksProvider, TscListProvider, GameConstructorWrapper);
        }
    }
}