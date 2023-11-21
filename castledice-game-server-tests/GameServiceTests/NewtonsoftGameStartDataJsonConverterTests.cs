using castledice_game_data_logic;
using castledice_game_data_logic.JSONConverters;
using castledice_game_server.GameService;
using Newtonsoft.Json;
using static  castledice_game_server_tests.ObjectCreationUtility;

namespace castledice_game_server_tests.GameServiceTests;

public class NewtonsoftGameStartDataJsonConverterTests
{
    [Fact]
    public void GetJson_ShouldReturnAppropriateJsonOfGivenGameStartData()
    {
        var expectedData = GetGameStartData();
        var converter = new NewtonsoftGameStartDataJsonConverter();

        var json = converter.GetJson(expectedData);
        var actualData = JsonConvert.DeserializeObject<GameStartData>(json, new ContentDataConverter(), new TscDataConverter());

        Assert.Equal(expectedData, actualData);
    }
}