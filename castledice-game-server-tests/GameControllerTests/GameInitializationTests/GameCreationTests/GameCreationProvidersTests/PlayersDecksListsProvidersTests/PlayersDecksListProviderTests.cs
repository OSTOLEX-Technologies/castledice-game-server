using castledice_game_logic.GameObjects;
using castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersDecksListsProviders;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameCreationTests.GameCreationProvidersTests.PlayersDecksListsProvidersTests;

public class PlayersDecksListProviderTests
{
    [Theory]
    [MemberData(nameof(GetPlayersDecksListTestsCases))]
    public void GetPlayersDecksList_ShouldReturnDecksList_WithDecksFromGivenProvider(Dictionary<int, List<PlacementType>> expectedPlayerIdsToDecks)
    {
        var deckProvider = new Mock<IPlayerDeckProvider>();
        foreach (var playerIdToDeck in expectedPlayerIdsToDecks)
        {
            deckProvider.Setup(x => x.GetDeckForPlayer(playerIdToDeck.Key)).Returns(playerIdToDeck.Value);
        }
        var deckListProvider = new PlayersDecksListProvider(deckProvider.Object);
        
        var decksList = deckListProvider.GetPlayersDecksList(expectedPlayerIdsToDecks.Keys.ToList());
        
        foreach (var playerIdToDeck in expectedPlayerIdsToDecks)
        {
            Assert.Same(playerIdToDeck.Value, decksList.GetDeck(playerIdToDeck.Key));
        }
    }

    public static IEnumerable<object[]> GetPlayersDecksListTestsCases()
    {
        yield return new object[]
        {
            new Dictionary<int, List<PlacementType>>
            {
                { 1, new List<PlacementType>() },
                { 2, new List<PlacementType>() }
            }
        };
        yield return new object[]
        {
            new Dictionary<int, List<PlacementType>>
            {
                { 3, new List<PlacementType> { PlacementType.Knight, PlacementType.HeavyKnight } },
                { 4, new List<PlacementType> { PlacementType.Knight, PlacementType.Knight } }
            }
        };
        yield return new object[]
        {
            new Dictionary<int, List<PlacementType>>
            {
                { 5, new List<PlacementType>() },
                { 6, new List<PlacementType>() },
                { 7, new List<PlacementType>() },
                { 8, new List<PlacementType>() }
            }
        };
    }
}