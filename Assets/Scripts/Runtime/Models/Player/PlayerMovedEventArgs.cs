using System;
using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Player
{
    /// <summary>
    /// Infos sur l'action de déplacement
    /// </summary>
    public sealed class PlayerMovedEventArgs : EventArgs
    {
        #region Propriétés

        /// <summary>
        /// Ancienne position
        /// </summary>
        public Vector3Int PreviousPos { get; }

        /// <summary>
        /// Nouvelle position
        /// </summary>
        public Vector3Int NewPos { get; }

        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="previousPos">Ancienne position</param>
        /// <param name="newPos">Nouvelle position</param>
        public PlayerMovedEventArgs(Vector3Int previousPos, Vector3Int newPos)
        {
            PreviousPos = previousPos;
            NewPos = newPos;
        }

        #endregion
    }
}