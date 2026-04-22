using System.Runtime.CompilerServices;
using Assets.Scripts.Runtime.Models.Actors;
using Assets.Scripts.Runtime.Models.ValueTypes;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Map
{
    /// <summary>
    /// Grille contenant les cases pour chaque couche de la carte
    /// </summary>
    public sealed class Grid
    {
        #region Propriétés

        /// <summary>
        /// Taille des tableaux générés
        /// </summary>
        public int Length => GridSize.x * GridSize.y;

        /// <summary>
        /// Dimensions de la grille
        /// </summary>
        public int2 GridSize { get; set; }

        /// <summary>
        /// Couche contenant les cases de l'environnment
        /// </summary>
        public StaticEnvironmentActor[] StaticEnvironmentLayer { get; set; }

        /// <summary>
        /// Couche contenant les cases des liquides
        /// </summary>
        public LiquidActor[] LiquidsLayer { get; set; }

        /// <summary>
        /// Couche contenant les cases des portes
        /// </summary>
        public DoorActor[] DoorsLayer { get; set; }

        /// <summary>
        /// Couche contenant les cases interactives
        /// </summary>
        public InteractableActor[] InteractablesLayer { get; set; }

        /// <summary>
        /// Liste des salles du niveau
        /// </summary>
        public DungeonStructure[] Rooms { get; set; }

        /// <summary>
        /// Liste des corridors du niveau
        /// </summary>
        public DungeonStructure[] Corridors { get; set; }

        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="gridSize">Dimensions de la grille</param>
        public Grid(int2 gridSize)
        {
            int length = gridSize.x * gridSize.y;
            GridSize = gridSize;
            StaticEnvironmentLayer = new StaticEnvironmentActor[length];
            LiquidsLayer = new LiquidActor[length];
            DoorsLayer = new DoorActor[length];
            InteractablesLayer = new InteractableActor[length];
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Convertit les coordonnées en index
        /// </summary>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ToIndex(in int2 coords)
        {
            return ToIndex(coords.x, coords.y);
        }

        /// <summary>
        /// Convertit les coordonnées en index
        /// </summary>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ToIndex(in int x, in int y)
        {
            return x + y * GridSize.x;
        }

        /// <summary>
        /// Convertit les coordonnées en index
        /// </summary>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int2 ToInt2(in int index)
        {
            return new int2(index % GridSize.x, index / GridSize.x);
        }

        /// <summary>
        /// Convertit les coordonnées en index
        /// </summary>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3Int ToV3Int(in int index)
        {
            return new Vector3Int(index % GridSize.x, index / GridSize.x, 0);
        }

        /// <summary>
        /// Indique si les coordonnées tombent en dehors de la grille
        /// </summary>
        /// <param name="coords">Les coordonnées</param>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool OutOfBounds(in Vector3Int coords)
        {
            return coords.x < 0 || coords.x >= GridSize.x || coords.y < 0 || coords.y >= GridSize.y;
        }

        /// <summary>
        /// Indique si les coordonnées tombent en dehors de la grille
        /// </summary>
        /// <param name="coords">Les coordonnées</param>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool OutOfBounds(in int2 coords)
        {
            return coords.x < 0 || coords.x >= GridSize.x || coords.y < 0 || coords.y >= GridSize.y;
        }

        /// <summary>
        /// Prend une case au hasard sur les bords de la carte pour la génération de rivière
        /// en tenant compte de la largeur de la rivière à générer
        /// </summary>
        /// <param name="randEdge">Côté de la carte sélectionné</param>
        /// <param name="width">Ecart des extrémités de la carte</param>
        /// <param name="offsetFromEdge">Décalage du bord de la carte</param>
        /// <param name="rand">Générateur d'aléatoire</param>
        /// <returns>Une coordonnée au hasard sur la carte</returns>
        [BurstCompile]
        public void GetPointOnMapEdge([AssumeRange(0, 4)] in int randEdge, in int width, in int offsetFromEdge, ref Unity.Mathematics.Random rand, out int2 result)
        {
            switch (randEdge)
            {
                // Bord droit
                case 0:
                    result = new int2(GridSize.x - 1 - offsetFromEdge, rand.NextInt(width, GridSize.y - 1 - width));
                    break;

                // Bord gauche
                case 1:
                    result = new int2(offsetFromEdge, rand.NextInt(width, GridSize.y - 1 - width));
                    break;

                // Bord haut
                case 2:
                    result = new int2(rand.NextInt(width, GridSize.x - 1 - width), GridSize.y - 1 - offsetFromEdge);
                    break;

                // Bord bas
                case 3:
                    result = new int2(rand.NextInt(width, GridSize.x - 1 - width), offsetFromEdge);
                    break;

                default:
                    goto case 0;
            }
        }

        #endregion
    }
}