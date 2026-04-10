using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Tiles
{
    /// <summary>
    /// Données d'une case représentant un élément de l'environnement (mur, sol, liquide, etc.)
    /// </summary>
    [CreateAssetMenu(fileName = "New Environment Tile", menuName = "Scriptable Objects/Tiles/Environment Tile SO")]
    public sealed class EnvironmentTileSO : TileEntitySO
    {
        [field: Space(10)]
        [field: Header("Environment")]
        [field: Space(10)]

        /// <summary>
        /// Les attributs de cette case
        /// </summary>
        [field: SerializeField]
        public EnvironmentTileLayerMask LayerMask { get; private set; }

        /// <summary>
        /// true si le joueur peut voir à travers cette case
        /// </summary>
        [field: SerializeField]
        public bool SeeThrough { get; private set; }
    }
}