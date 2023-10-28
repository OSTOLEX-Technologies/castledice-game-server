using castledice_game_logic;
using castledice_game_logic.GameObjects;

namespace castledice_game_server.GameController.GameInitialization.GameCreation;

public interface IPlayersDecksProvider
{
    IDecksList GetPlayersDecksList(List<Player> players);
}