using castledice_game_logic.BoardGeneration.CellsGeneration;

namespace castledice_game_server.GameController.GameInitialization.GameCreation.GameCreationProviders.BoardConfigProviders.CellsGeneratorsProviders;

public interface ICellsGeneratorProvider
{
    ICellsGenerator GetCellsGenerator();
}