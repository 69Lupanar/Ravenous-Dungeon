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
        public int2 MinMaxNbRooms { get; private set; }

        /// <summary>
        /// L'intervalle possible de la taille des salles à instancier
        /// </summary>
        [field: SerializeField]
        public int2 MinMaxRoomSize { get; private set; }
    }
}