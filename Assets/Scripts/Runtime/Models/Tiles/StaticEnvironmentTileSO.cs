using Assets.Scripts.Runtime.Models.ValueTypes;
using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Tiles
{
    /// <summary>
    /// Données d'une case représentant un élément statique de l'environnement (mur, sol etc.)
    /// </summary>
    [CreateAssetMenu(fileName = "New Environment Tile", menuName = "Scriptable Objects/Tiles/Environment Tile")]
    public sealed class StaticEnvironmentTileSO : TileEntitySO
    {
        [field: Space(10)]
        [field: Header("Environment")]
        [field: Space(10)]

        /// <summary>
        /// Les attributs de cette case
        /// </summary>
        [field: SerializeField]
        public EnvironmentTileLayerMask LayerMask { get; set; }
    }
}