using castledice_game_data_logic;
using castledice_game_data_logic.JSONConverters;
using Newtonsoft.Json;

namespace castledice_game_server.GameService;

public class NewtonsoftGameStartDataJsonConverter : IGameStartDataJsonConverter
{
    public string GetJson(GameStartData data)
    {
        return JsonConvert.SerializeObject(data, new ContentDataConverter());
    }
}