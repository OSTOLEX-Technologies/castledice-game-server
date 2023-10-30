using castledice_game_data_logic;
using castledice_game_logic.GameObjects;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Providers;
using Moq;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameStartDataCreationTests;

public class DecksDataProviderTests
{
    [Theory]
    [MemberData(nameof(GetPlayersDeckDataTestCases))]
    public void GetPlayersDeckData_ShouldReturnListOfAppropriatePlayerDeckData(IDecksList deckList, List<int> playersIds,
        List<PlayerDeckData> expectedDataList)
    {
        var provider = new DecksDataProvider();
        
        var actualDataList = provider.GetPlayersDecksData(deckList, playersIds);
        
        Assert.Equal(expectedDataList, actualDataList);
    }

    public static IEnumerable<object[]> GetPlayersDeckDataTestCases()
    {
        yield return new object[]
        {
            GetProvider(new Dictionary<int, List<PlacementType>>
            {
                { 1, new List<PlacementType> { PlacementType.Knight, PlacementType.HeavyKnight } },
                { 2, new List<PlacementType> { PlacementType.Knight, PlacementType.HeavyKnight } },
                { 3, new List<PlacementType> { PlacementType.Knight } },
                { 4, new List<PlacementType> { PlacementType.Knight } },
            }),
            new List<int>{1, 2, 3, 4},
            new List<PlayerDeckData>
            {
                new PlayerDeckData(1, new List<PlacementType> { PlacementType.Knight, PlacementType.HeavyKnight }),
                new PlayerDeckData(2, new List<PlacementType> { PlacementType.Knight, PlacementType.HeavyKnight }),
                new PlayerDeckData(3, new List<PlacementType> { PlacementType.Knight }),
                new PlayerDeckData(4, new List<PlacementType> { PlacementType.Knight })
            }
        };
        yield return new object[]
        {
            GetProvider(new Dictionary<int, List<PlacementType>>
            {
                { 13, new List<PlacementType> { PlacementType.HeavyKnight } },
                { 153, new List<PlacementType> { PlacementType.Knight } }
            }),
            new List<int>{13, 153},
            new List<PlayerDeckData>
            {
                new PlayerDeckData(13, new List<PlacementType> { PlacementType.HeavyKnight }),
                new PlayerDeckData(153, new List<PlacementType> { PlacementType.Knight })
            }
        };
    }

    private static IDecksList GetProvider(Dictionary<int, List<PlacementType>> playersIdsToDecks)
    {
        var mock = new Mock<IDecksList>();
        foreach (var idsToDeck in playersIdsToDecks)
        {
            mock.Setup(x => x.GetDeck(idsToDeck.Key)).Returns(idsToDeck.Value);
        }
        
        return mock.Object;
    } 
}