using System;
using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Tiles.TilePalette
{
    /// <summary>
    /// Associe à un sprite une chance d'être sélectionné
    /// pour afficher sa case correspondante
    /// </summary>
    /// <typeparam name="T">Le type de donnée</typeparam>
    [Serializable]
    public struct ItemSpawnChance<T>
    {
        /// <summary>
        /// Le sprite de la case
        /// </summary>
        [field: SerializeField]
        public T Value { get; private set; }

        /// <summary>
        /// Le % de chance d'être sélectionné
        /// </summary>
        [field: SerializeField]
        public float Chance { get; private set; }
    }
}