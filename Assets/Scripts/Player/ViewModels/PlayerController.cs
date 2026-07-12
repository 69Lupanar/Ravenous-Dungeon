using System;
using UnityEngine;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Chargé d'instancier et contrôler le joueur
    /// après la génération d'un niveau
    /// </summary>
    public sealed class PlayerController : MonoBehaviour
    {
        #region Evénements

        /// <summary>
        /// Appelé quand le joueur déplace le personnage
        /// </summary>
        public EventHandler<PlayerMovedEventArgs> OnPlayerMoved;

        /// <summary>
        /// Appelé quand le personnage est placé sur la carte
        /// </summary>
        public EventHandler<PlayerSpawnedEventArgs> OnPlayerSpawned;

        #endregion

        #region Propriétés

        /// <summary>
        /// La position du joueur
        /// </summary>
        public Vector3Int PlayerPos { get; private set; }

        #endregion

        #region Variables Unity

        /// <summary>
        /// Les inputs du joueur
        /// </summary>
        [SerializeField]
        private PlayerInput _input;

        /// <summary>
        /// Indique les types de cases où le joueur peut être instancié
        /// </summary>
        [SerializeField]
        private LayerMask _playerSpawnMask;

        /// <summary>
        /// Indique les types de cases naviguables par le joueur
        /// </summary>
        [SerializeField]
        private LayerMask _playerWalkableMask;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// màj à chaque frame
        /// </summary>
        private void Update()
        {
            if (_input.RequestedMoveThisFrame)
                MovePlayer();
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Instancie le joueur à une case correspondant
        /// à son layerMask de cases naviguables
        /// </summary>
        /// <param name="grid">Grille contenant les cases</param>
        /// <param name="rand">Générateur d'aléatoire</param>
        public void SpawnPlayer(Grid grid, ref Unity.Mathematics.Random rand)
        {
            // TAF : Placer le joueur sur la grille

            OnPlayerSpawned?.Invoke(this, new PlayerSpawnedEventArgs(PlayerPos));
        }

        /// <summary>
        /// Indique si la case est naviguable par le joueur
        /// </summary>
        /// <param name="layerMask">Les attributs de la case</param>
        public bool TileIsWalkable(LayerMask layerMask)
        {
            return _playerWalkableMask == layerMask;
        }

        /// <summary>
        /// Déplace le joueur sur la grille
        /// </summary>
        private void MovePlayer()
        {
            Vector3Int dest = PlayerPos + new Vector3Int(_input.MoveDirection.x, _input.MoveDirection.y, 0);
            Vector3Int previousPos = PlayerPos;
            PlayerPos = dest;

            // TAF: Déplacer le joueur

            OnPlayerMoved?.Invoke(this, new PlayerMovedEventArgs(previousPos, dest));
        }

        #endregion
    }
}