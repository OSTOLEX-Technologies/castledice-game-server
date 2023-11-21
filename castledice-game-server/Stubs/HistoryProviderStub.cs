using castledice_game_logic;
using castledice_game_server.GameController.GameOver;

namespace castledice_game_server.Stubs;

/// <summary>
/// This class MUST NOT be used in production build.
/// </summary>
public class HistoryProviderStub : IHistoryProvider
{
    public string GetGameHistory(Game game)
    {
        return "History provider is not implemented yet.";
    }
}