using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.MapGeneration.Algorithms
{
    /// <summary>
    /// Param�tres de g�n�ration de la carte
    /// </summary>
    [CreateAssetMenu(fileName = "Rooms And Corridors Algorithm Settings", menuName = "Scriptable Objects/Castle of Temptation/Map Generation/Rooms And Corridors Algorithm Settings")]
    public class RoomsAndCorridorsAlgorithmSettingsSO : MapGenerationAlgorithmSettingsSO
    {
        /// <summary>
        /// L'intervalle possible du nombre max de salles à instancier
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("L'intervalle possible du nombre max de salles à instancier")]
        public int2 NbRoomsInterval { get; private set; }

        /// <summary>
        /// L'intervalle possible de la taille des salles à instancier
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("L'intervalle possible de la taille des salles à instancier")]
        public int2 RoomSizeInterval { get; private set; }

        /// <summary>
        /// %age de chance possible de placer une porte lorsque
        /// le cas se présente
        /// </summary>
        [field: Tooltip("%age de chance possible de placer une porte lorsque le cas se présente")]
        [field: SerializeField, Range(0f, 100f)]
        public int DoorSpawnRate { get; private set; }
    }
}