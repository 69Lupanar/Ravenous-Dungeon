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
        public int2 GridSize { get; set; }

        /// <summary>
        /// Couche contenant les cases de l'environnment
        /// </summary>
        public TileSO[] EnvironmentLayer { get; set; }

        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="gridSize">Dimensions de la grille</param>
        public Grid(int2 gridSize)
        {
            GridSize = gridSize;
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

        #endregion
    }
}