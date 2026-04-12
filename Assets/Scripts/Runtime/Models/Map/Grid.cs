using Assets.Scripts.Runtime.Models.Actors;
using Assets.Scripts.Runtime.Models.ValueTypes;
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
        public int ToIndex(int2 coords)
        {
            return ToIndex(coords.x, coords.y);
        }

        /// <summary>
        /// Convertit les coordonnées en index
        /// </summary>
        public int ToIndex(int x, int y)
        {
            return x + y * GridSize.x;
        }

        /// <summary>
        /// Convertit les coordonnées en index
        /// </summary>
        public int2 ToInt2(int index)
        {
            return new int2(index % GridSize.x, index / GridSize.x);
        }

        /// <summary>
        /// Convertit les coordonnées en index
        /// </summary>
        public Vector3Int ToV3Int(int index)
        {
            return new Vector3Int(index % GridSize.x, index / GridSize.x, 0);
        }

        /// <summary>
        /// Indique si les coordonnées tombent en dehors de la grille
        /// </summary>
        /// <param name="coords">Les coordonnées</param>
        public bool OutOfBounds(Vector3Int coords)
        {
            return coords.x < 0 || coords.x >= GridSize.x || coords.y < 0 || coords.y >= GridSize.y;
        }

        #endregion
    }
}