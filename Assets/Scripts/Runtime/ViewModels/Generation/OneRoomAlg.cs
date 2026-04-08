using Assets.Scripts.Runtime.Models.Tiles;
using Assets.Scripts.Runtime.Models.Tiles.TilePalette;
using Unity.Mathematics;

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
        /// <param name="tileLibrary">Contient les cases utilisés pour la génération</param>
        /// <param name="gridSize">Les dimensions de la grille</param>
        /// <returns>La grille des cases créées</returns>
        public static TileSO[] Generate(TileLibrarySO tileLibrary, int2 gridSize)
        {
            TileSO[] environmentLayer = new TileSO[gridSize.x * gridSize.y];

            // Remplit la carte de murs pour pouvoir en creuser les salles

            GenerationAlgUtils.FillMap(environmentLayer, gridSize, tileLibrary.WallTile);

            // Creuse une unique salle qui remplit tout l'étage

            GenerationAlgUtils.CreateRectangularRoom(gridSize, new(1, 1), new(gridSize.x - 2, gridSize.y - 2), tileLibrary, environmentLayer);

            return environmentLayer;
        }
    }
}