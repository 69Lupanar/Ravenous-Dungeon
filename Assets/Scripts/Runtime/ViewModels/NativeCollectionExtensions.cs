using System;
using Unity.Burst;
using Unity.Collections;

namespace Assets.Scripts.Runtime.ViewModels
{
    /// <summary>
    /// Extensions pour les NativeCollections
    /// </summary>
    public static class NativeCollectionExtensions
    {
        /// <summary>
        /// Inverse la collection
        /// </summary>
        /// <typeparam name="T">Type de la collection</typeparam>
        /// <param name="arr">La collection à inverser</param>
        [BurstCompile]
        public static void Reverse<T>(ref this NativeArray<T> arr) where T : unmanaged
        {
            for (int i = 0; i < arr.Length / 2; ++i)
            {
                (arr[i], arr[arr.Length - 1 - i]) = (arr[arr.Length - 1 - i], arr[i]);
            }
        }

        /// <summary>
        /// Inverse la collection
        /// </summary>
        /// <typeparam name="T">Type de la collection</typeparam>
        /// <param name="arr">La collection à inverser</param>
        [BurstCompile]
        public static void Reverse<T>(ref this NativeList<T> arr) where T : unmanaged
        {
            for (int i = 0; i < arr.Length / 2; ++i)
            {
                (arr[i], arr[arr.Length - 1 - i]) = (arr[arr.Length - 1 - i], arr[i]);
            }
        }

        /// <summary>  
        /// Indique si la collection contient la valeur demandée
        /// </summary>
        /// <typeparam name="T">Type de la collection</typeparam>
        /// <param name="arr">La collection à inverser</param>
        /// <param name="value">La valeur recherchée</param>
        [BurstCompile]
        public static bool Contains<T>(ref this NativeArray<T> arr, T value) where T : unmanaged, IEquatable<T>
        {
            for (int i = 0; i < arr.Length; ++i)
            {
                if (arr[i].Equals(value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Indique si la collection contient la valeur demandée
        /// </summary>
        /// <typeparam name="T">Type de la collection</typeparam>
        /// <param name="arr">La collection à inverser</param>
        /// <param name="value">La valeur recherchée</param>
        [BurstCompile]
        public static bool Contains<T>(ref this NativeList<T> arr, T value) where T : unmanaged, IEquatable<T>
        {
            for (int i = 0; i < arr.Length; ++i)
            {
                if (arr[i].Equals(value))
                {
                    return true;
                }
            }

            return false;
        }
    }
}