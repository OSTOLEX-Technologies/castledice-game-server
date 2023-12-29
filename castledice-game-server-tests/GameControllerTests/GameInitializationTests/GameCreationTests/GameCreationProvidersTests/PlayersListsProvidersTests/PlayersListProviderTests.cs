using castledice_game_logic.Time;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersListProviders;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.PlayersListsProvidersTests;

public class PlayersListProviderTests
{
    [Fact]
    public void GetPlayersList_ShouldReturnListOfPlayers_WithAppropriateIds()
    {
        var idsList = GetIdsListWithRandomLength();
        var provider = new PlayersListProvider(new Mock<IPlayerTimerProvider>().Object);
        
        var players = provider.GetPlayersList(idsList);
        
        Assert.Equal(idsList.Count, players.Count);
        foreach (var id in idsList)
        {
            Assert.Contains(players, p => p.Id == id);
        }
    }
    
    [Fact]
    public void GetPlayersList_ShouldReturnPlayers_WithTimerFromProvider()
    {
        var expectedTimer = new Mock<IPlayerTimer>();
        var timerProviderMock = new Mock<IPlayerTimerProvider>();
        timerProviderMock.Setup(x => x.GetPlayerTimer()).Returns(expectedTimer.Object);
        var provider = new PlayersListProvider(timerProviderMock.Object);
        
        var players = provider.GetPlayersList(GetIdsListWithRandomLength());
        
        foreach (var player in players)
        {
            Assert.Same(expectedTimer.Object, player.Timer);
        }
    }
    
    private static List<int> GetIdsListWithRandomLength()
    {
        var random = new Random();
        var count = random.Next(1, 10);
        return GetIdsList(count);
    }
    
    private static List<int> GetIdsList(int count)
    {
        var ids = new List<int>();
        for (var i = 0; i < count; i++)
        {
            ids.Add(i);
        }

        return ids;
    }
}