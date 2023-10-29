﻿using castledice_game_logic.GameObjects;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.PlayersDecksListsProviders;

public interface IPlayersDecksProvider
{
    IDecksList GetPlayersDecksList(List<int> playersIds);
}