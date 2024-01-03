using castledice_game_logic.GameObjects;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.Creators.PlayersListCreators.PlayerCreators.PlayersDecksCreators;

public interface IPlayerDeckCreator
{
    List<PlacementType> GetDeckForPlayer(int playerId);
}