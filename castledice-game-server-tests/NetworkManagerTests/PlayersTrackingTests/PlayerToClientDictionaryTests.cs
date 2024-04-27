using castledice_game_server.NetworkManager.PlayersTracking;

namespace castledice_game_server_tests.NetworkManagerTests.PlayersTrackingTests;

public class PlayerToClientDictionaryTests
{
    [Fact]
    public void GetClientIdForPlayer_ShouldThrowInvalidOperationException_IfNoClientIdForGivenPlayerId()
    {
        var dictionary = new PlayerToClientIdsMap();
        
        Assert.Throws<InvalidOperationException>(() => dictionary.GetClientIdForPlayer(1));
    }
    
    [Fact]
    public void SaveClientIdForPlayer_ShouldSaveClientIdForGivenPlayerId()
    {
        var dictionary = new PlayerToClientIdsMap();
        dictionary.SaveClientIdForPlayer(1, 2);
        
        Assert.Equal(2, dictionary.GetClientIdForPlayer(1));
    }
    
    [Fact]
    public void SaveClientIdForPlayer_ShouldThrowInvalidOperationException_IfClientIdAlreadySavedForGivenPlayerId()
    {
        var dictionary = new PlayerToClientIdsMap();
        dictionary.SaveClientIdForPlayer(1, 2);
        
        Assert.Throws<InvalidOperationException>(() => dictionary.SaveClientIdForPlayer(1, 3));
    }
    
    [Fact]
    public void RemoveClientIdForPlayer_ShouldRemoveClientIdForGivenPlayerId()
    {
        var dictionary = new PlayerToClientIdsMap();
        dictionary.SaveClientIdForPlayer(1, 2);
        dictionary.RemoveClientIdForPlayer(1);
        
        Assert.Throws<InvalidOperationException>(() => dictionary.GetClientIdForPlayer(1));
    }
    
    [Fact]
    public void RemoveClientIdForPlayer_ShouldReturnFalse_IfNoClientIdForGivenPlayerId()
    {
        var dictionary = new PlayerToClientIdsMap();
        
        Assert.False(dictionary.RemoveClientIdForPlayer(1));
    }
    
    [Fact]
    public void RemoveClientIdForPlayer_ShouldReturnTrue_IfClientIdForGivenPlayerId()
    {
        var dictionary = new PlayerToClientIdsMap();
        dictionary.SaveClientIdForPlayer(1, 2);
        
        Assert.True(dictionary.RemoveClientIdForPlayer(1));
    }
    
    [Fact]
    public void PlayerHasClientId_ShouldReturnFalse_IfNoClientIdForGivenPlayerId()
    {
        var dictionary = new PlayerToClientIdsMap();
        
        Assert.False(dictionary.PlayerHasClientId(1));
    }
    
    [Fact]
    public void PlayerHasClientId_ShouldReturnTrue_IfClientIdForGivenPlayerId()
    {
        var dictionary = new PlayerToClientIdsMap();
        dictionary.SaveClientIdForPlayer(1, 2);
        
        Assert.True(dictionary.PlayerHasClientId(1));
    }
}