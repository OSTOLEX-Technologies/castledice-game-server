using castledice_game_data_logic;
using castledice_game_logic;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators.PlayerDataCreators;

public class PlayersDataListCreator : IPlayersDataListCreator
{
    private readonly IPlayerDataCreator _playerDataCreator;

    public PlayersDataListCreator(IPlayerDataCreator playerDataCreator)
    {
        _playerDataCreator = playerDataCreator;
    }

    public List<PlayerData> GetPlayersData(List<Player> players)
    {
        return players.Select(player => _playerDataCreator.GetPlayerData(player)).ToList();
    }
}