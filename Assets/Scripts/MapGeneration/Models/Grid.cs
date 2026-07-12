using Unity.Mathematics;

namespace Assets.Scripts.MapGeneration
{
    /// <summary>
    /// Grille contenant les cases du terrain et des acteurs
    /// </summary>
    public class Grid
    {
        #region Propriétés

        /// <summary>
        /// Dimensions de la carte à générer
        /// </summary>
        public int2 Size { get; set; }

        #endregion
    }
}