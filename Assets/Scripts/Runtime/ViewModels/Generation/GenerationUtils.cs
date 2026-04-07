using Assets.Scripts.Runtime.Models.Generation;
using Assets.Scripts.Runtime.Models.Tiles;
using Assets.Scripts.Runtime.Models.Tiles.Map;
using Assets.Scripts.Runtime.Models.Tiles.TilePalette;
using Unity.Mathematics;

namespace Assets.Scripts.Runtime.ViewModels.Generation
{
    /// <summary>
    /// Contient les méthodes des algorithmes de génération
    /// </summary>
    public static class GenerationUtils
    {
        #region Méthodes publiques

        /// <summary>
        /// Génère une nouvelle carte
        /// </summary>
        /// <param name="gs">Paramètres de génération</param>
        /// <param name="tl">Contient les cases utilisés pour la génération</param>
        /// <param name="alg">Algorithme de génération sélectionné</param>
        public static Grid Generate(GenerationSettingsSO gs, TileLibrarySO tl, GenerationAlgorithmType alg)
        {
            return alg switch
            {
                GenerationAlgorithmType.OneRoom => OneRoomAlg.Generate(gs, tl),
                GenerationAlgorithmType.RoomsAndCorridors => RoomsAndCorridorsAlg.Generate(gs, tl),
                _ => new(int2.zero, null),
            };
        }

        #region Dungeon Structures

        /// <summary>
        /// Remplit la carte entière d'un seul type de case
        /// </summary>
        public static void FillMap(TileSO[] environmentLayer, int2 gridSize, TileSO tile)
        {
            for (int y = 0; y < gridSize.y; ++y)
            {
                for (int x = 0; x < gridSize.x; ++x)
                {
                    environmentLayer[x + y * gridSize.x] = tile;
                }
            }
        }

        /// <summary>
        /// Génère une structure rectangulaire
        /// </summary>
        /// <param name="gridSize">Dimensions de la grille</param>
        /// <param name="position">La position de la structure</param>
        /// <param name="dimensions">Les dimensions de la structure</param>
        /// <param name="tileLibrary">Contient les cases utilisés pour la génération</param>
        /// <param name="environmentLayer">Couche de la tilemap représentant l'environnement</param>
        public static void CreateRectangularRoom(int2 gridSize, int2 position, int2 dimensions, TileLibrarySO tileLibrary, TileSO[] environmentLayer)
        {
            for (int y = position.y; y < position.y + dimensions.y; ++y)
            {
                for (int x = position.x; x < position.x + dimensions.x; ++x)
                {
                    environmentLayer[x + y * gridSize.x] = tileLibrary.GroundTile;
                }
            }
        }


        /// <summary>
        /// Carve a tunnel out of the map parallel to the x-axis
        /// </summary>
        /// <param name="gridSize"></param>
        /// <param name="xStart"></param>
        /// <param name="xEnd"></param>
        /// <param name="yPosition"></param>
        /// <param name="tileLibrary"></param>
        /// <param name="environmentLayer"></param>
        public static void CreateHorizontalTunnel(int2 gridSize, int xStart, int xEnd, int yPosition, TileLibrarySO tileLibrary, TileSO[] environmentLayer)
        {
            for (int x = math.min(xStart, xEnd); x <= math.max(xStart, xEnd); ++x)
            {
                environmentLayer[x + yPosition * gridSize.x] = tileLibrary.GroundTile;
            }
        }

        /// <summary>
        /// Carve a tunnel out of the map parallel to the y-axis
        /// </summary>
        /// <param name="gridSize"></param>
        /// <param name="yStart"></param>
        /// <param name="yEnd"></param>
        /// <param name="xPosition"></param>
        /// <param name="tileLibrary"></param>
        /// <param name="environmentLayer"></param>
        public static void CreateVerticalTunnel(int2 gridSize, int yStart, int yEnd, int xPosition, TileLibrarySO tileLibrary, TileSO[] environmentLayer)
        {
            for (int y = math.min(yStart, yEnd); y <= math.max(yStart, yEnd); ++y)
            {
                environmentLayer[xPosition + y * gridSize.x] = tileLibrary.GroundTile;
            }
        }

        #endregion

        #endregion
    }
}