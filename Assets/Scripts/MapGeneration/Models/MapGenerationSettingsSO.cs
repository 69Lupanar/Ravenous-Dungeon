using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.MapGeneration
{
    /// <summary>
    /// Paramètres de génération d'une carte
    /// </summary>
    [CreateAssetMenu(fileName = "New Map Generation Settings", menuName = "Scriptable Objects/Catle of Temptation/Generation/Map Generation Settings")]
    public sealed class MapGenerationSettingsSO : ScriptableObject
    {
        /// <summary>
        /// Dimensions de la carte à générer
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("Dimensions de la carte à générer")]
        public int2 Size { get; private set; }
    }
}