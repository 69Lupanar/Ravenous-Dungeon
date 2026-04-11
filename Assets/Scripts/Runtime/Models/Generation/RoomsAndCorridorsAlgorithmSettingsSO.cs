using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Generation
{
    /// <summary>
    /// Paramètres de génération de la carte
    /// </summary>
    [CreateAssetMenu(fileName = "Rooms And Corridors Algorithm Settings", menuName = "Scriptable Objects/Generation/Rooms And Corridors Algorithm Settings")]
    public class RoomsAndCorridorsAlgorithmSettingsSO : GenerationAlgorithmSettingsSO
    {
        /// <summary>
        /// L'intervalle possible du nombre max de salles à instancier
        /// </summary>
        [field: SerializeField]
        public int2 NbRoomsInterval { get; private set; }

        /// <summary>
        /// L'intervalle possible de la taille des salles à instancier
        /// </summary>
        [field: SerializeField]
        public int2 RoomSizeInterval { get; private set; }

        /// <summary>
        /// %age de chance possible de placer une porte lorsque
        /// le cas se présente
        /// </summary>
        [field: SerializeField, Range(0f, 100f)]
        public int DoorSpawnRate { get; private set; }
    }
}