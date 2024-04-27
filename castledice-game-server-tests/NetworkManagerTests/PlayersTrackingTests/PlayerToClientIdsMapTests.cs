using castledice_game_server.NetworkManager.PlayersTracking;

namespace castledice_game_server_tests.NetworkManagerTests.PlayersTrackingTests;

public class PlayerToClientIdsMapTests
{
    private readonly Random _random = new();
    
    [Fact]
    public void GetPlayerByClient_ShouldReturnPlayerId_IfItWasSavedForGivenClientId()
    {
        var map = new PlayerToClientIdsMap();
        var playerId = _random.Next();
        var clientId = (ushort)_random.Next();
        map.SaveClientToPlayer(playerId, clientId);
        
        var result = map.GetPlayerByClient(clientId);
        
        Assert.Equal(playerId, result);
    }
    
    [Fact]
    public void SaveClientToPlayer_ShouldThrowInvalidOperationException_IfClientIdAlreadyExists()
    {
        var map = new PlayerToClientIdsMap();
        var playerId = _random.Next();
        var clientId = (ushort)_random.Next();
        map.SaveClientToPlayer(playerId, clientId);
        
        Assert.Throws<InvalidOperationException>(() => map.SaveClientToPlayer(playerId + 1, clientId));
    }
    
    [Fact]
    public void SaveClientToPlayer_ShouldThrowInvalidOperationException_IfPlayerIdAlreadyExists()
    {
        var map = new PlayerToClientIdsMap();
        var playerId = _random.Next();
        var clientId = (ushort)_random.Next();
        map.SaveClientToPlayer(playerId, clientId);
        
        Assert.Throws<InvalidOperationException>(() => map.SaveClientToPlayer(playerId, (ushort)(clientId + 1)));
    }
    
    [Fact]
    public void GetPlayerByClient_ShouldThrowInvalidOperationException_IfClientIdWasNotSaved()
    {
        var map = new PlayerToClientIdsMap();
        var clientId = (ushort)_random.Next();
        
        Assert.Throws<InvalidOperationException>(() => map.GetPlayerByClient(clientId));
    }
    
    [Fact]
    public void GetClientByPlayer_ShouldReturnClientId_IfItWasSavedForGivenPlayerId()
    {
        var map = new PlayerToClientIdsMap();
        var playerId = _random.Next();
        var clientId = (ushort)_random.Next();
        map.SaveClientToPlayer(playerId, clientId);
        
        var result = map.GetClientByPlayer(playerId);
        
        Assert.Equal(clientId, result);
    }
    
    [Fact]
    public void GetClientByPlayer_ShouldThrowInvalidOperationException_IfPlayerIdWasNotSaved()
    {
        var map = new PlayerToClientIdsMap();
        var playerId = _random.Next();
        
        Assert.Throws<InvalidOperationException>(() => map.GetClientByPlayer(playerId));
    }
    
    [Fact]
    public void PlayerHasClient_ShouldReturnTrue_IfClientIdWasSavedForGivenPlayerId()
    {
        var map = new PlayerToClientIdsMap();
        var playerId = _random.Next();
        var clientId = (ushort)_random.Next();
        map.SaveClientToPlayer(playerId, clientId);
        
        var result = map.PlayerHasClient(playerId);
        
        Assert.True(result);
    }
    
    [Fact]
    public void PlayerHasClient_ShouldReturnFalse_IfClientIdWasNotSavedForGivenPlayerId()
    {
        var map = new PlayerToClientIdsMap();
        var playerId = _random.Next();
        
        var result = map.PlayerHasClient(playerId);
        
        Assert.False(result);
    }
    
    [Fact]
    public void ClientHasPlayer_ShouldReturnTrue_IfPlayerIdWasSavedForGivenClientId()
    {
        var map = new PlayerToClientIdsMap();
        var playerId = _random.Next();
        var clientId = (ushort)_random.Next();
        map.SaveClientToPlayer(playerId, clientId);
        
        var result = map.ClientHasPlayer(clientId);
        
        Assert.True(result);
    }
    
    [Fact]
    public void ClientHasPlayer_ShouldReturnFalse_IfPlayerIdWasNotSavedForGivenClientId()
    {
        var map = new PlayerToClientIdsMap();
        var clientId = (ushort)_random.Next();
        
        var result = map.ClientHasPlayer(clientId);
        
        Assert.False(result);
    }
    
    [Fact]
    public void RemovePairByClient_ShouldReturnTrue_IfPairWasRemoved()
    {
        var map = new PlayerToClientIdsMap();
        var playerId = _random.Next();
        var clientId = (ushort)_random.Next();
        map.SaveClientToPlayer(playerId, clientId);
        
        var result = map.RemovePairByClient(clientId);
        
        Assert.True(result);
    }
    
    [Fact]
    public void RemovePairByClient_ShouldReturnFalse_IfPairWasNotRemoved()
    {
        var map = new PlayerToClientIdsMap();
        var clientId = (ushort)_random.Next();
        
        var result = map.RemovePairByClient(clientId);
        
        Assert.False(result);
    }
    
    [Fact]
    public void RemovePairByPlayer_ShouldReturnTrue_IfPairWasRemoved()
    {
        var map = new PlayerToClientIdsMap();
        var playerId = _random.Next();
        var clientId = (ushort)_random.Next();
        map.SaveClientToPlayer(playerId, clientId);
        
        var result = map.RemovePairByPlayer(playerId);
        
        Assert.True(result);
    }
    
    [Fact]
    public void RemovePairByPlayer_ShouldReturnFalse_IfPairWasNotRemoved()
    {
        var map = new PlayerToClientIdsMap();
        var playerId = _random.Next();
        
        var result = map.RemovePairByPlayer(playerId);
        
        Assert.False(result);
    }
}