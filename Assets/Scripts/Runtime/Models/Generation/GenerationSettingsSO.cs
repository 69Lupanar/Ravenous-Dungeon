using Assets.Scripts.Runtime.Models.Generation.Algorithms;
using Assets.Scripts.Runtime.Models.Tiles.TilePalette;
using Assets.Scripts.Runtime.Models.ValueTypes;
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
        [field: Space(10)]
        [field: Header("Rendering")]
        [field: Space(10)]

        /// <summary>
        /// Détermine l'apparence des cases à utiliser pour cette génération.
        /// Permet d'utiliser différents sprites pour différents biomes.
        /// </summary>
        [field: SerializeField]
        public SpriteLibrarySO SpriteLibrary { get; private set; }

        [field: Header("Generation")]
        [field: Space(10)]

        /// <summary>
        /// L'intervalle possible de dimensions de la grille à générer
        /// </summary>
        [field: SerializeField]
        public int2 GridSizeInterval { get; private set; }

        /// <summary>
        /// Détermine les cases à utiliser pour cette génération.
        /// Permet d'utiliser différents cases pour différents biomes
        /// (par ex, des arbres au lieu de murs pour un biome de forêt)
        /// </summary>
        [field: SerializeField]
        public TileLibrarySO TileLibrary { get; private set; }

        /// <summary>
        /// Paramètre de génération des rivières et lacs
        /// </summary>
        [field: SerializeField]
        public ItemSelectionChance<RiverGenerationSettingsSO>[] RiverGenerationSettings { get; private set; }

        /// <summary>
        /// La liste des algos de génération acceptés.
        /// Permet de restreindre les algorithmes à certains biomes.
        /// </summary>
        [field: SerializeField]
        public ItemSelectionChance<GenerationAlgorithmSettingsSO>[] Algorithms { get; private set; }
    }
}