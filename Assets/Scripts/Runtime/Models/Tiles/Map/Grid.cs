using Assets.Scripts.Runtime.Models.Features;
using Unity.Mathematics;

namespace Assets.Scripts.Runtime.Models.Tiles.Map
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
        public int2 GridSize { get; private set; }

        /// <summary>
        /// Couche contenant les cases de l'environnment
        /// </summary>
        public TileSO[] EnvironmentLayer { get; private set; }

        /// <summary>
        /// Représente les salles créées lors de la génération
        /// </summary>
        public DungeonStructure[] Rooms { get; private set; }

        /// <summary>
        /// Représente les coulois créés lors de la génération
        /// </summary>
        public DungeonStructure[] Corridors { get; private set; }

        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="gridSize">Dimensions de la grille</param>
        /// <param name="environmentLayer">Couche contenant les cases de l'environnment</param>
        public Grid(int2 gridSize, TileSO[] environmentLayer)
        {
            GridSize = gridSize;
            EnvironmentLayer = environmentLayer;
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Convertit les coordonnées en index
        /// </summary>
        public int ToIndex(int x, int y)
        {
            return x + y * GridSize.x;
        }

        /// <summary>
        /// Enregistre les salles du niveau
        /// </summary>
        /// <param name="rooms">Les salles du niveau</param>
        public void SetRooms(DungeonStructure[] rooms)
        {
            Rooms = rooms;
        }

        /// <summary>
        /// Enregistre les corridors du niveau
        /// </summary>
        /// <param name="rooms">Les corridors du niveau</param>
        public void SetCorridors(DungeonStructure[] corridors)
        {
            Corridors = corridors;
        }

        #endregion
    }
}