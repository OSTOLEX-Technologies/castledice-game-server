using castledice_game_logic.BoardGeneration.CellsGeneration;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders;

public interface ICellsGeneratorProvider
{
    ICellsGenerator GetCellsGenerator();
}