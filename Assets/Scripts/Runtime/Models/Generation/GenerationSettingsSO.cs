using Assets.Scripts.Runtime.Models.Tiles.TilePalette;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Generation
{
    /// <summary>
    /// Paramètres de génération de la carte
    /// </summary>
    [CreateAssetMenu(fileName = "New Generation Settings", menuName = "Scriptable Objects/Generation/Generation Settings", order = 0)]
    public sealed class GenerationSettingsSO : ScriptableObject
    {
        /// <summary>
        /// L'intervalle possible de dimensions de la grille à générer
        /// </summary>
        [field: SerializeField]
        public int2 MinMaxGridSize { get; private set; }

        /// <summary>
        /// La liste des algos de génération acceptés.
        /// Permet de restreindre les algorithmes à certains biomes.
        /// </summary>
        [field: SerializeField]
        public ItemSelectionChance<GenerationAlgorithmSettingsSO>[] Algorithms { get; private set; }

        /// <summary>
        /// Détermine l'apparence des cases à utiliser pour cette génération.
        /// Permet d'utiliser différents sprites pour différents biomes.
        /// </summary>
        [field: SerializeField]
        public SpriteLibrarySO SpriteLibrary { get; private set; }

        /// <summary>
        /// Détermine les cases à utiliser pour cette génération.
        /// Permet d'utiliser différents cases pour différents biomes
        /// (par ex, des arbres au lieu de murs pour un biome de forêt)
        /// </summary>
        [field: SerializeField]
        public TileLibrarySO TileLibrary { get; private set; }

    }
}