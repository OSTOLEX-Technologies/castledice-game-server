using castledice_game_logic.GameObjects;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators.PlayerDataCreators;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameStartDataCreationTests.CreatorsTests.PlayerDataCreatorsTests;

public class PlayerDataCreatorTests
{
    [Fact]
    public void GetPlayerData_ShouldReturnPlayerData_WithGivenPlayerId()
    {
        var expectedId = new Random().Next();
        var player = GetPlayer(expectedId);
        var creator = new PlayerDataCreator();
        
        var playerData = creator.GetPlayerData(player);
        
        Assert.Equal(expectedId, playerData.PlayerId);
    }

    [Fact]
    public void GetPlayerData_ShouldReturnPlayerData_WithGivenPlayerTimeSpan()
    {
        var expectedTimeSpan = GetRandomTimeSpan();
        var player = GetPlayer(timeSpan: expectedTimeSpan);
        var creator = new PlayerDataCreator();
        
        var playerData = creator.GetPlayerData(player);
        
        Assert.Equal(expectedTimeSpan, playerData.TimeSpan);
    }
    
    [Fact]
    public void GetPlayerData_ShouldReturnPlayerData_WithGivenPlayerDeck()
    {
        var expectedDeck = GetRandomPlacementTypeList();
        var player = GetPlayer(deck: expectedDeck.ToArray());
        var creator = new PlayerDataCreator();
        
        var playerData = creator.GetPlayerData(player);
        
        Assert.Equal(expectedDeck, playerData.AvailablePlacements);
    }
}