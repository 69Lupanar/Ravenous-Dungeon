using Assets.Scripts.Runtime.Models.ValueTypes;
using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Tiles
{
    /// <summary>
    /// Représente les propriétés d'une case (mur, sol, etc.)
    /// </summary>
    public class TileEntitySO : ScriptableObject
    {
        [field: Header("General")]
        [field: Space(10)]

        /// <summary>
        /// Attributs de la case
        /// </summary>
        [field: SerializeField]
        public TileAttributes Attributes { get; private set; }

        /// <summary>
        /// La description de la case
        /// </summary>
        [field: SerializeField, TextArea(3, 5)]
        public string Description { get; private set; }
    }
}