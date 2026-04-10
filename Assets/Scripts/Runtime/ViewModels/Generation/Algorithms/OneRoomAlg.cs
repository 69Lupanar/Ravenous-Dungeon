using Assets.Scripts.Runtime.Models.Features;
using Assets.Scripts.Runtime.Models.Map;
using Assets.Scripts.Runtime.Models.Tiles;
using Assets.Scripts.Runtime.Models.Tiles.TilePalette;
using Unity.Mathematics;

namespace Assets.Scripts.Runtime.ViewModels.Generation.Algorithms
{
    /// <summary>
    /// Algorithme générant une seule salle recouvrant toute la carte
    /// </summary>
    public static class OneRoomAlg
    {
        /// <summary>
        /// Génère une salle recouvrant toute la carte
        /// </summary>
        /// <param name="tileLibrary">Contient les cases utilisés pour la génération</param>
        /// <param name="grid">La grille</param>
        /// <returns>La grille des cases créées</returns>
        public static void GenerateEnvironmnent(TileLibrarySO tileLibrary, Grid grid)
        {
            TileEntitySO[] environmentLayer = new TileEntitySO[grid.GridSize.x * grid.GridSize.y];

            // Remplit la carte de murs pour pouvoir en creuser les salles

            GenerationAlgUtils.FillMap(environmentLayer, grid.GridSize, tileLibrary.WallTile);

            // Creuse une unique salle qui remplit tout l'étage

            GenerationAlgUtils.CreateRectangularRoom(grid.GridSize, new(1, 1), new(grid.GridSize.x - 2, grid.GridSize.y - 2), tileLibrary, environmentLayer);

            grid.EnvironmentLayer = environmentLayer;
            grid.Rooms = new DungeonStructure[1] { new(new int2(1, 1), grid.GridSize - new int2(2, 2)) };
            grid.Corridors = new DungeonStructure[0];
        }
    }
}