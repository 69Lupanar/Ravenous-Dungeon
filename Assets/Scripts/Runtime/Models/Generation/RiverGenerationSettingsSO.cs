using Assets.Scripts.Runtime.Models.ValueTypes;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Generation
{
    /// <summary>
    /// Paramètres de génération des rivières et lacs
    /// </summary>
    [CreateAssetMenu(fileName = "New River Generation Settings", menuName = "Scriptable Objects/Generation/River Generation Settings")]
    public class RiverGenerationSettingsSO : ScriptableObject
    {
        /// <summary>
        /// L'intervalle possible de dimensions de la grille à générer
        /// </summary>
        [field: SerializeField]
        public int2 NbRiversInterval { get; private set; }

        /// <summary>
        /// %age de chance qu'au mois 1 rivière soit créée lors de la génération
        /// </summary>
        [field: SerializeField, Range(0f, 100f)]
        public float RiverSpawnRate { get; private set; }

        /// <summary>
        /// L'intervalle possible de largeur des rivières générées
        /// </summary>
        [field: SerializeField]
        public int2 RiverWidthInterval { get; private set; }

        /// <summary>
        /// La liste des types de rivières acceptées par le biome
        /// </summary>
        [field: SerializeField]
        public ItemSelectionChance<RiverType>[] RiverTypes { get; private set; }

    }
}