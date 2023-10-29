using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersListProviders;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.PlayersListsProvidersTests;

public class PlayersListProviderTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(1, 2)]
    [InlineData(1, 2, 3)]
    [InlineData(1, 2, 3, 4)]
    [InlineData(1, 2, 3, 4, 5)]
    public void GetPlayersList_ShouldReturnListOfPlayers_WithAppropriateIds(params int[] ids)
    {
        var idsList = ids.ToList();
        var provider = new PlayersListProvider();
        
        var players = provider.GetPlayersList(idsList);
        
        Assert.Equal(idsList.Count, players.Count);
        foreach (var id in ids)
        {
            Assert.Contains(players, p => p.Id == id);
        }
    }
}