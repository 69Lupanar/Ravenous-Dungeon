using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.MapGeneration
{
    /// <summary>
    /// Grille contenant les cases du terrain et des acteurs
    /// </summary>
    public static class Grid
    {
        #region Propriétés

        /// <summary>
        /// Dimensions de la carte à générer
        /// </summary>
        public static int2 Size { get; set; }

        #endregion
    }
}