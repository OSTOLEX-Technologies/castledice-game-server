﻿using castledice_game_data_logic;
using castledice_game_data_logic.ConfigsData;
using castledice_game_data_logic.Content;
using castledice_game_logic;
using castledice_game_logic.ActionPointsLogic;
using castledice_game_logic.BoardGeneration.CellsGeneration;
using castledice_game_logic.BoardGeneration.ContentGeneration;
using castledice_game_logic.GameConfiguration;
using castledice_game_logic.GameObjects;
using castledice_game_logic.GameObjects.Configs;
using castledice_game_logic.GameObjects.Factories.Castles;
using castledice_game_logic.Math;

namespace castledice_game_server_tests;

public class ObjectCreationUtility
{
    public static GameStartData GetGameStartData()
    {
        return GetGameStartData(1, 2);
    }

    public static Player GetPlayer(int id)
    {
        return new Player(new PlayerActionPoints(), id);
    }
    
    public static GameStartData GetGameStartData(params int[] playerIds)
    {
        var version = "1.0.0";
        var boardData = GetBoardData();
        var playersIdsList = new List<int>(playerIds);
        var placeablesConfigs = new PlaceablesConfigData(new KnightConfigData(1, 2));
        var playerDecks = new List<PlayerDeckData>()
        {
            new(playerIds[0], new List<PlacementType> { PlacementType.Knight }),
            new (playerIds[1], new List<PlacementType> { PlacementType.Knight })
        };
        var data = new GameStartData(version, boardData, placeablesConfigs, playersIdsList, playerDecks);
        return data;
    }

    public static BoardData GetBoardData(CellType cellType = CellType.Square)
    {
        var cellsPresence = GetValuesMatrix(10, 10, true);
        return GetBoardData(cellsPresence, cellType);
    }
        
    public static BoardData GetBoardData(bool[,] cellsPresence, CellType cellType = CellType.Square)
    {
        var boardLength = 10;
        var boardWidth = 10;
        var firstCastle = new CastleData((0, 0), 1, 1, 3, 3, 1);
        var secondCastle = new CastleData((9, 9), 1, 1, 3, 3, 2);
        var generatedContent = new List<ContentData>
        {
            firstCastle, 
            secondCastle
        };
        return new BoardData(boardLength, boardWidth, cellType, cellsPresence, generatedContent);
    }
    
    public static T[,] GetValuesMatrix<T>(int length, int width, T value)
    {
        var matrix = new T[length, width];
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                matrix[i, j] = value;
            }
        }

        return matrix;
    }

    public static Game GetGame()
    {
        var firstPlayer = new Player(new PlayerActionPoints(), 1);
        var secondPlayer = new Player(new PlayerActionPoints(), 2);
        return GetGame(firstPlayer, secondPlayer);
    }
    
    public static Game GetGame(Player firstPlayer, Player secondPlayer)
    {
        var players = new List<Player>()
        {
            firstPlayer,
            secondPlayer
        };

        var playersToCastlesPositions = new Dictionary<Player, Vector2Int>()
        {
            { firstPlayer, (0, 0) },
            { secondPlayer, (9, 9) }
        };
        var castleConfig = new CastleConfig(3, 1, 1);
        var castlesFactory = new CastlesFactory(castleConfig);
        var casltesSpawner = new CastlesSpawner(playersToCastlesPositions, castlesFactory);

        var contentSpanwers = new List<IContentSpawner>()
        {
            casltesSpawner
        };

        var cellsGenerator = new RectCellsGenerator(10, 10);
            
        var boardConfig = new BoardConfig(contentSpanwers, cellsGenerator, CellType.Square);

        var unitsConfig = new PlaceablesConfig(new KnightConfig(1, 2));

        var placementListProvider = new CommonDecksList(new List<PlacementType>
        {
            PlacementType.Knight
        });
            
        var game = new Game(players, boardConfig, unitsConfig, placementListProvider);

        return game;
    }

    public static BoardConfig GetBoardConfig()
    {
        var playersToCastlesPositions = new Dictionary<Player, Vector2Int>()
        {
            { new Player(new PlayerActionPoints(), 1), (0, 0) },
            { new Player(new PlayerActionPoints(), 2), (9, 9) }
        };
        return GetBoardConfig(playersToCastlesPositions);
    }
    
    public static BoardConfig GetBoardConfig(Dictionary<Player, Vector2Int> playersToCastlesPositions)
    {
        var castleConfig = new CastleConfig(3, 1, 1);
        var castlesFactory = new CastlesFactory(castleConfig);
        var castlesSpawner = new CastlesSpawner(playersToCastlesPositions, castlesFactory);

        var contentSpawners = new List<IContentSpawner>
        {
            castlesSpawner
        };

        var cellsGenerator = new RectCellsGenerator(10, 10);
            
        var boardConfig = new BoardConfig(contentSpawners, cellsGenerator, CellType.Square);

        return boardConfig;
    }
    
    public static PlaceablesConfig GetPlaceablesConfig()
    {
        var unitsConfig = new PlaceablesConfig(new KnightConfig(1, 2));
        return unitsConfig;
    }
}