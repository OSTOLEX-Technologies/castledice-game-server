using castledice_game_logic.GameObjects;
using castledice_game_logic.Time;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators.PlayerCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators.PlayerCreators.PlayersDecksCreators;
using castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators.PlayerCreators.PlayerTimerCreators;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.CreatorsTests.PlayersListsCreatorsTests.PlayerCreatorsTests;

public class PlayerCreatorTests
{
    [Fact]
    public void GetPlayer_ShouldReturnPlayer_WithGivenId()
    {
        var expectedId = new Random().Next();
        var creator = new PlayerCreatorBuilder().Build();
        
        var player = creator.GetPlayer(expectedId);
        
        Assert.Equal(expectedId, player.Id);
    }

    [Fact]
    public void GetPlayer_ShouldReturnPlayer_WithDeckFromCreator()
    {
        var playerId = new Random().Next();
        var expectedDeck = new List<PlacementType>();
        var deckCreatorMock = new Mock<IPlayerDeckCreator>();
        deckCreatorMock.Setup(x => x.GetDeckForPlayer(playerId)).Returns(expectedDeck);
        var creator = new PlayerCreatorBuilder{PlayerDeckCreator = deckCreatorMock.Object}.Build();
        
        var player = creator.GetPlayer(playerId);
        
        Assert.Equal(expectedDeck, player.Deck);
    }

    [Fact]
    public void GetPlayer_ShouldReturnPlayer_WithTimerFromCreator()
    {
        var expectedTimer = new Mock<IPlayerTimer>().Object;
        var timerCreatorMock = new Mock<IPlayerTimerCreator>();
        timerCreatorMock.Setup(x => x.GetPlayerTimer()).Returns(expectedTimer);
        var creator = new PlayerCreatorBuilder{PlayerTimerCreator = timerCreatorMock.Object}.Build();
        
        var player = creator.GetPlayer(new Random().Next());
        
        Assert.Equal(expectedTimer, player.Timer);
    }
    
    private class PlayerCreatorBuilder
    {
        public IPlayerDeckCreator PlayerDeckCreator { get; set; } = new Mock<IPlayerDeckCreator>().Object;
        public IPlayerTimerCreator PlayerTimerCreator { get; set; } = new Mock<IPlayerTimerCreator>().Object;
        
        public PlayerCreator Build()
        {
            return new(PlayerDeckCreator, PlayerTimerCreator);
        }
    }
}