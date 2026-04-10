using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Tiles
{
    /// <summary>
    /// Représente les propriétés d'une case (mur, sol, etc.)
    /// </summary>
    public class TileEntitySO : ScriptableObject
    {
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

        /// <summary>
        /// La description de la case
        /// </summary>
        [field: SerializeField, TextArea(3, 5)]
        public string Description { get; private set; }
    }
}