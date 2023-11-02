using castledice_game_server.Auth;

namespace castledice_game_server.Stubs;

/// <summary>
/// This class MUST NOT be used in production build.
/// </summary>
public class StringIdRetrieverStub : IIdRetriever
{
    public async Task<int> RetrievePlayerIdAsync(string playerToken)
    {
        return int.Parse(playerToken);
    }
}