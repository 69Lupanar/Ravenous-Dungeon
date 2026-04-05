using Assets.Scripts.Runtime.Models.Tiles;
using Assets.Scripts.Runtime.Models.Tiles.Generation;
using Assets.Scripts.Runtime.Models.Tiles.Map;
using Assets.Scripts.Runtime.Models.Tiles.TilePalette;
using Unity.Mathematics;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Runtime.ViewModels.Generation
{
    /// <summary>
    /// Contient les méthodes des algorithmes de génération
    /// </summary>
    public static class GenerationAlgorithms
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
            int2 gridSize = new(Random.Range(gs.MinMaxGridSize.x, gs.MinMaxGridSize.y),
                                Random.Range(gs.MinMaxGridSize.x, gs.MinMaxGridSize.y));

            return alg switch
            {
                GenerationAlgorithmType.OneRoom => GenerateOneRoom(gridSize, tl),
                _ => new(int2.zero, null),
            };
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Génère une salle recouvrant toute la carte
        /// </summary>
        /// <param name="gridSize">Dimensions de la grille</param>
        /// <param name="tileLibrary">Contient les cases utilisés pour la génération</param>
        private static Grid GenerateOneRoom(in int2 gridSize, TileLibrarySO tileLibrary)
        {
            TileSO[] environmentLayer = new TileSO[gridSize.x * gridSize.y];

            int count = 0;

            for (int i = 0; i < gridSize.x; ++i)
            {
                environmentLayer[i] = tileLibrary.WallTile;
            }

            count += gridSize.x;

            for (int i = 1; i < gridSize.y - 1; ++i)
            {
                environmentLayer[count] = tileLibrary.WallTile;

                for (int j = 1; j < gridSize.x; ++j)
                {
                    environmentLayer[count + j] = tileLibrary.GroundTile;
                }

                environmentLayer[count + gridSize.x - 1] = tileLibrary.WallTile;

                count += gridSize.x;
            }

            for (int i = count; i < count + gridSize.x; ++i)
            {
                environmentLayer[i] = tileLibrary.WallTile;
            }


            return new Grid(gridSize, environmentLayer);
        }

        #endregion
    }
}