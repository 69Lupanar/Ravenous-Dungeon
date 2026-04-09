using System;
using System.Collections.Generic;
using Unity.Mathematics;
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
        #region Variables d'instance

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

        #endregion

        #region Méthodes publiques statiques

        /// <summary>
        /// Evalue les chances de chaque élément de la collection
        /// et obtient un élément au hasard
        /// </summary>
        /// <param name="collection">La liste d'éléments</param>
        /// <returns>Un élément choisi au hasard</returns>
        public static T Evaluate(IEnumerable<ItemSpawnChance<T>> collection)
        {
            float rand = UnityEngine.Random.Range(0f, 100f);
            float2 curInterval = new(0f, 0f);
            T result = default;

            foreach (ItemSpawnChance<T> item in collection)
            {
                curInterval.y += item.Chance;

                if (curInterval.x < rand && rand < curInterval.y)
                {
                    result = item.Value;
                    break;
                }

                curInterval.x += item.Chance;
            }

            return result;
        }

        #endregion
    }
}