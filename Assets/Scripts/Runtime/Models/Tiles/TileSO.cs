using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Tiles
{
    /// <summary>
    /// Représente les propriétés d'une case (mur, sol, etc.)
    /// </summary>
    public class TileSO : ScriptableObject
    {
        /// <summary>
        /// La description de la case
        /// </summary>
        [field: SerializeField, TextArea(3, 5)]
        public string Description { get; private set; }
    }
}