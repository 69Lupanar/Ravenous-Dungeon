using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Runtime.Models.Tiles.TilePalette;
using Unity.Mathematics;

namespace Assets.Scripts.Runtime.ViewModels.Extensions
{
    /// <summary>
    /// Méthodes d'extension
    /// </summary>
    public static class ItemSelectionChanceExtensions
    {
        /// <summary>
        /// Evalue les chances de chaque élément de la collection
        /// et obtient un élément au hasard
        /// </summary>
        /// <param name="collection">La liste d'éléments</param>
        /// <returns>Un élément choisi au hasard</returns>
        public static T Sample<T>(this IEnumerable<ItemSelectionChance<T>> collection)
        {
            if (collection.Count() == 1)
            {
                return collection.First().Value;
            }

            float rand = UnityEngine.Random.Range(0f, 100f);
            float2 curInterval = new(0f, 0f);
            T result = default;

            foreach (ItemSelectionChance<T> item in collection)
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

    }
}