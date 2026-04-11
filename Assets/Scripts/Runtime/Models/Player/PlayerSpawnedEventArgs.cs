using System;
using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Player
{
    /// <summary>
    /// Infos sur la génération du personnage
    /// </summary>
    public sealed class PlayerSpawnedEventArgs : EventArgs
    {
        #region Propriétés

        /// <summary>
        /// Position du joueur
        /// </summary>
        public Vector3Int PlayerPosition { get; }

        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="position">Position du joueur</param>
        public PlayerSpawnedEventArgs(Vector3Int position)
        {
            PlayerPosition = position;
        }

        #endregion
    }
}