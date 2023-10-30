using castledice_game_data_logic.ConfigsData;
using castledice_game_logic.GameConfiguration;
using castledice_game_logic.GameObjects.Configs;
using castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Providers;

namespace castledice_game_server_tests.GameControllerTests.GameInitializationTests.GameStartDataCreationTests;

public class PlaceablesConfigDataProviderTest
{
    [Theory]
    [MemberData(nameof(GetPlaceablesConfigDataTestCases))]
    public void GetPlaceablesConfigData_ShouldReturnAppropriatePlaceablesConfigData(PlaceablesConfig config,
        PlaceablesConfigData expectedData)
    {
        var provider = new PlaceablesConfigDataProvider();
        
        var actualData = provider.GetPlaceablesConfigData(config);
        
        Assert.Equal(expectedData, actualData);
    }

    public static IEnumerable<object[]> GetPlaceablesConfigDataTestCases()
    {
        yield return new object[]
        {
            new PlaceablesConfig(new KnightConfig(1, 2)),
            new PlaceablesConfigData(new KnightConfigData(1, 2))
        };
        yield return new object[]
        {
            new PlaceablesConfig(new KnightConfig(3, 4)),
            new PlaceablesConfigData(new KnightConfigData(3, 4))
        };
        yield return new object[]
        {
            new PlaceablesConfig(new KnightConfig(5, 6)),
            new PlaceablesConfigData(new KnightConfigData(5, 6))
        };
    }
}