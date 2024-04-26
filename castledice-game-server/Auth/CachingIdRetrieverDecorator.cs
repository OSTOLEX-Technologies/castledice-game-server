namespace castledice_game_server.Auth;

public class CachingIdRetrieverDecorator : IIdRetriever
{
    private readonly IIdRetriever _retriever;
    private readonly Dictionary<string, int> _cache = new();

    public CachingIdRetrieverDecorator(IIdRetriever retriever)
    {
        _retriever = retriever;
    }

    public async Task<int> RetrievePlayerIdAsync(string playerToken)
    {
        if (_cache.TryGetValue(playerToken, out var id))
        {
            return id;
        }
        id = await _retriever.RetrievePlayerIdAsync(playerToken);
        _cache[playerToken] = id;
        return id;        
    }
}