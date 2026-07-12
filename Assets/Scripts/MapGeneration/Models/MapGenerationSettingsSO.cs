using Assets.Scripts.MapGeneration.Algorithms;
using Assets.Scripts.TilePalettes;
using Assets.Scripts.ValueTypes;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.MapGeneration
{
    /// <summary>
    /// Paramètres de génération d'une carte
    /// </summary>
    [CreateAssetMenu(fileName = "New Map Generation Settings", menuName = "Scriptable Objects/Castle of Temptation/Map Generation/Map Generation Settings")]
    public sealed class MapGenerationSettingsSO : ScriptableObject
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
        /// L'intervalle possible de dimensions de la grille � g�n�rer
        /// </summary>
        [field: SerializeField]
        public int2 GridSizeInterval { get; private set; }

        /// <summary>
        /// La liste des algos de g�n�ration accept�s.
        /// Permet de restreindre les algorithmes � certains biomes.
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("Dimensions de la carte à générer")]
        public ItemSelectionChance<GenerationAlgorithmSettingsSO>[] Algorithms { get; private set; }
    }
}