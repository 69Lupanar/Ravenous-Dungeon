using Assets.Scripts.Runtime.Models.Tiles.TilePalette;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Tiles.Generation
{
    /// <summary>
    /// Paramètres de génération de la carte
    /// </summary>
    [CreateAssetMenu(fileName = "Generation Settings", menuName = "Scriptable Objects/Generation/Generation Settings")]
    public sealed class GenerationSettingsSO : ScriptableObject
    {
        /// <summary>
        /// L'intervalle possible de dimensions de la grille à générer
        /// </summary>
        [field: SerializeField]
        public int2 MinMaxGridSize { get; private set; }

        /// <summary>
        /// Les biomes pouvant être générés
        /// </summary>
        [field: SerializeField]
        public BiomeTilePaletteSO[] Biomes { get; private set; }
    }
}