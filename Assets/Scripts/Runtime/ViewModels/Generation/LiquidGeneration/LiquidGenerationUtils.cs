using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;

namespace Assets.Scripts.Runtime.ViewModels.Generation.LiquidGeneration
{
    /// <summary>
    /// Génère les rivières et lacs de la carte
    /// </summary>
    [BurstCompile]
    public static class LiquidGenerationUtils
    {
        #region Constants

        /// <summary>
        /// Coût de navigation d'une case
        /// </summary>
        private const int NORMAL_WEIGHT = 10;

        #endregion

        #region Méthodes internes

        /// <summary>
        /// Génère une rivière
        /// </summary>
        /// <param name="startPos">Début de la rivière</param>
        /// <param name="endPos">Fin de la rivière</param>
        /// <param name="gridSize">Dimensions de la grille</param>
        /// <param name="rand">Générateur d'aléatoire</param>
        ///  <param name="result">Liste des coordonnées où placer des cases de liquide</param>
        [BurstCompile, SkipLocalsInit]
        internal static void CreateRiver(in int width, in int2 startPos, in int2 endPos, in NativeArray<int2>.ReadOnly directions, in int2 gridSize, ref Random rand, out NativeArray<int2> result)
        {
            result = new NativeArray<int2>(0, Allocator.Temp);
        }

        /// <summary>
        /// Génère un lac
        /// </summary>
        /// <param name="width">Largeur du lac</param>
        /// <param name="center">Centre du lac</param>
        /// <param name="directions">Directions possibles dans lesquelles étendre le lac</param>
        /// <param name="gridSize">Dimensions de la grille</param>
        /// <param name="rand">Générateur d'aléatoire</param>
        ///  <param name="coords">Liste des coordonnées où placer des cases de liquide</param>
        [BurstCompile]
        internal static void CreateLake(in int width, in int2 center,
                                        in NativeArray<int2>.ReadOnly directions, in int2 gridSize, ref Unity.Mathematics.Random rand, out Unity.Collections.NativeArray<int2> result)
        {
            result = new NativeArray<int2>(0, Allocator.Temp);
        }

        /// <summary>
        /// Prend une case au hasard sur les bords de la carte pour la génération de rivière
        /// en tenant compte de la largeur de la rivière à générer
        /// </summary>
        /// <param name="randEdge">Côté de la carte sélectionné</param>
        /// <param name="width">Ecart des extrémités de la carte</param>
        /// <param name="gridSize">Dimensions de la grille</param>
        /// <param name="rand">Générateur d'aléatoire</param>
        /// <returns>Une coordonnée au hasard sur la carte</returns>
        [BurstCompile]
        internal static void GetPointOnMapEdge(in int randEdge, in int width, in int2 gridSize, ref Random rand, out int2 result)
        {
            switch (randEdge)
            {
                // Bord droit
                case 0:
                    result = new int2(gridSize.x - 1, rand.NextInt(width, gridSize.y - width));
                    break;

                // Bord gauche
                case 1:
                    result = new int2(1, rand.NextInt(width, gridSize.y - width));
                    break;

                // Bord haut
                case 2:
                    result = new int2(rand.NextInt(width, gridSize.x - width), gridSize.y - 1);
                    break;

                // Bord bas
                case 3:
                    result = new int2(rand.NextInt(width, gridSize.x - width), 1);
                    break;

                default:
                    goto case 0;
            }
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Obtient la position de la node dans la liste
        /// </summary>
        /// <param name="pos">Position</param>
        /// <param name="gridSizeX">Largeur de la grille</param>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int ToIndex(in int2 pos, in int gridSizeX)
        {
            return pos.x + pos.x * gridSizeX;
        }

        #endregion
    }
}