using castledice_game_data_logic;
using castledice_game_logic;

namespace castledice_game_server.GameController.GameInitialization.GameStartDataCreation.Creators.PlayerDataCreators;

public interface IPlayerDataCreator
{
    PlayerData GetPlayerData(Player player);
}