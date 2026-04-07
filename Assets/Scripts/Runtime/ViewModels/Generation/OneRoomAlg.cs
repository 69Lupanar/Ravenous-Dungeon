using Assets.Scripts.Runtime.Models.Features;
using Assets.Scripts.Runtime.Models.Generation;
using Assets.Scripts.Runtime.Models.Tiles;
using Assets.Scripts.Runtime.Models.Tiles.Map;
using Assets.Scripts.Runtime.Models.Tiles.TilePalette;
using Unity.Mathematics;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Runtime.ViewModels.Generation
{
    /// <summary>
    /// Algorithme générant une seule salle recouvrant toute la carte
    /// </summary>
    public static class OneRoomAlg
    {
        /// <summary>
        /// Génère une salle recouvrant toute la carte
        /// </summary>
        /// <param name="gs">Paramètres de génération</param>
        /// <param name="tileLibrary">Contient les cases utilisés pour la génération</param>
        /// <returns>La grille des cases créées</returns>
        public static Grid Generate(GenerationSettingsSO gs, TileLibrarySO tileLibrary)
        {
            int2 gridSize = new(Random.Range(gs.MinMaxGridSize.x, gs.MinMaxGridSize.y),
                                Random.Range(gs.MinMaxGridSize.x, gs.MinMaxGridSize.y));

            TileSO[] environmentLayer = new TileSO[gridSize.x * gridSize.y];
            DungeonStructure room = new(new int2(1, 1), gridSize - new int2(gridSize.x - 2, gridSize.y - 2));
            GenerationUtils.CreateRectangularRoom(gridSize, room.Position, room.Dimensions, tileLibrary, environmentLayer);

            Grid grid = new(gridSize, environmentLayer);
            grid.SetRooms(new DungeonStructure[1] { room });
            grid.SetCorridors(new DungeonStructure[0]);
            return grid;
        }
    }
}