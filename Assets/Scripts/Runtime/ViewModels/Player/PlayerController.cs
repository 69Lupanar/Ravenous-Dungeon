using System;
using Assets.Scripts.Runtime.Models.Actors;
using Assets.Scripts.Runtime.Models.Player;
using Assets.Scripts.Runtime.Models.ValueTypes;
using UnityEngine;
using Grid = Assets.Scripts.Runtime.Models.Map.Grid;

namespace Assets.Scripts.Runtime.ViewModels.Player
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
        private EnvironmentTileLayerMask _playerSpawnMask;

        /// <summary>
        /// Indique les types de cases naviguables par le joueur
        /// </summary>
        [SerializeField]
        private EnvironmentTileLayerMask _playerWalkableMask;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// Grille contenant les cases
        /// </summary>
        private Grid _grid;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// màj à chaque frame
        /// </summary>
        private void Update()
        {
            if (_input.RequestedMoveThisFrame && _grid != null)
            {
                MovePlayer();
            }
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Assigne la grille pour permettre 
        /// de se déplacer en fonction de l'environnement
        /// </summary>
        /// <param name="grid">Grille contenant les cases</param>
        public void SetGrid(Grid grid)
        {
            _grid = grid;
        }

        /// <summary>
        /// Instancie le joueur à une case correspondant
        /// à son layerMask de cases naviguables
        /// </summary>
        /// <param name="grid">Grille contenant les cases</param>
        /// <param name="rand">Générateur d'aléatoire</param>
        public void SpawnPlayer(Grid grid, ref Unity.Mathematics.Random rand)
        {
            int index;

            do
            {
                // On cherche un emplacement libre et qui n'ait pas été remplacé par un liquide
                // (càd que le Data de l'acteur n'est pas null)

                index = rand.NextInt(grid.StaticEnvironmentLayer.Length);
            }
            while (grid.StaticEnvironmentLayer[index].Data == null ^ !_playerSpawnMask.HasFlag(grid.StaticEnvironmentLayer[index].LayerMask));

            PlayerPos = grid.ToV3Int(index);

            OnPlayerSpawned?.Invoke(this, new PlayerSpawnedEventArgs(PlayerPos));
        }

        /// <summary>
        /// Indique si la case est naviguable par le joueur
        /// </summary>
        /// <param name="layerMask">Les attributs de la case</param>
        public bool TileIsWalkable(EnvironmentTileLayerMask layerMask)
        {
            return _playerWalkableMask.HasFlag(layerMask);
        }

        /// <summary>
        /// Déplace le joueur sur la grille
        /// </summary>
        private void MovePlayer()
        {
            Vector3Int dest = PlayerPos + new Vector3Int(_input.MoveDirection.x, _input.MoveDirection.y, 0);

            if (!_grid.OutOfBounds(dest))
            {
                // On ne se déplace que s'il y a une surface naviguable à la destination
                // (càd un environnement statique, OU un liquide) 
                StaticEnvironmentActor destTile1 = _grid.StaticEnvironmentLayer[_grid.ToIndex(dest.x, dest.y)];
                LiquidActor destTile2 = _grid.LiquidsLayer[_grid.ToIndex(dest.x, dest.y)];

                if (destTile1.Data != null && TileIsWalkable(destTile1.LayerMask) ^ destTile2.Data != null && TileIsWalkable(destTile2.LayerMask))
                {
                    Vector3Int previousPos = PlayerPos;
                    PlayerPos = dest;

                    OnPlayerMoved?.Invoke(this, new PlayerMovedEventArgs(previousPos, dest));
                }
            }
        }

        #endregion
    }
}