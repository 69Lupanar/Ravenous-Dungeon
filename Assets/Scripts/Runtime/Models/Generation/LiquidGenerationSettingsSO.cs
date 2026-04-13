using Assets.Scripts.Runtime.Models.ValueTypes;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Generation
{
    /// <summary>
    /// Paramètres de génération des rivières et lacs
    /// </summary>
    [CreateAssetMenu(fileName = "New Liquid Generation Settings", menuName = "Scriptable Objects/Generation/Liquid Generation Settings")]
    public class LiquidGenerationSettingsSO : ScriptableObject
    {
        /// <summary>
        /// L'intervalle possible du nombre de rivières à générer
        /// </summary>
        [field: SerializeField]
        public int2 NbRiversInterval { get; private set; }

        /// <summary>
        /// L'intervalle possible du nombre de lacs à générer
        /// </summary>
        [field: SerializeField]
        public int2 NbLakesInterval { get; private set; }

        /// <summary>
        /// L'intervalle possible de branches que peut avoir une rivière
        /// </summary>
        [field: SerializeField]
        public int2 NbRiversForksInterval { get; private set; }

        /// <summary>
        /// %age de chance qu'au mois 1 rivière soit créée lors de la génération
        /// </summary>
        [field: SerializeField, Range(0f, 100f)]
        public float RiverSpawnRate { get; private set; }

        /// <summary>
        /// %age de chance qu'une rivière possède au moins 1 branche
        /// </summary>
        [field: SerializeField, Range(0f, 100f)]
        public float RiverForkSpawnRate { get; private set; }

        /// <summary>
        /// %age de chance qu'au mois 1 lac soit créée lors de la génération
        /// </summary>
        [field: SerializeField, Range(0f, 100f)]
        public float LakeSpawnRate { get; private set; }

        /// <summary>
        /// L'intervalle possible de largeur des rivières générées
        /// </summary>
        [field: SerializeField]
        public int2 RiverWidthInterval { get; private set; }

        /// <summary>
        /// L'intervalle possible de largeur des lacs générées
        /// </summary>
        [field: SerializeField]
        public int2 LakeWidthInterval { get; private set; }

        /// <summary>
        /// La liste des types de liquides acceptées par le biome
        /// </summary>
        [field: SerializeField]
        public ItemSelectionChance<LiquidType>[] LiquidTypes { get; private set; }

    }
}