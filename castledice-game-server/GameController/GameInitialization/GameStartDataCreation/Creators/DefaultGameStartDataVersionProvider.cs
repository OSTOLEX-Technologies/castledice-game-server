namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Providers;

public class DefaultGameStartDataVersionProvider : IGameStartDataVersionProvider
{
    private readonly string _version;

    public DefaultGameStartDataVersionProvider(string version)
    {
        _version = version;
    }

    public string GetGameStartDataVersion()
    {
        return _version;
    }
}