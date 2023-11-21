using castledice_game_data_logic;
using castledice_game_server.Exceptions;

namespace castledice_game_server.GameRepository;

public class LocalGameDataRepository : ILocalGameDataRepository
{
    private readonly List<GameData> _gamesData = new();
    
    public void AddGameData(GameData data)
    {
        if (!_gamesData.Contains(data))
        {
            _gamesData.Add(data);
        }
    }

    public GameData GetGameData(int id)
    {
        var gameData = _gamesData.FirstOrDefault(g => g.Id == id);
        if (gameData == null)
        {
            throw new GameDataNotFoundException($"Game data with id {id} not found.");
        }
        return gameData;
    }

    public bool RemoveGameData(int id)
    {
        var gameData = _gamesData.FirstOrDefault(g => g.Id == id);
        if (gameData == null)
        {
            return false;
        }
        _gamesData.Remove(gameData);
        return true;
    }
}