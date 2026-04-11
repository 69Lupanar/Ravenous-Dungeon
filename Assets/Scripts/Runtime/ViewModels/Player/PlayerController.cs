using System;
using Assets.Scripts.Runtime.Models.Player;
using Assets.Scripts.Runtime.Models.Tiles;
using Assets.Scripts.Runtime.Models.ValueTypes;
using Unity.Mathematics;
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
            if (_grid != null)
            {
                Vector3Int newPos = PlayerPos + new Vector3Int(_input.MoveDirection.x, _input.MoveDirection.y, 0);

                if (!_grid.OutOfBounds(newPos))
                {
                    EnvironmentTileSO destTile = _grid.EnvironmentLayer[_grid.ToIndex(newPos.x, newPos.y)];

                    if (TileIsWalkable(destTile.LayerMask))
                    {
                        Vector3Int previousPos = PlayerPos;
                        PlayerPos = newPos;

                        OnPlayerMoved?.Invoke(this, new PlayerMovedEventArgs(previousPos, newPos));
                    }
                }
            }

            _input.MoveDirection = int2.zero;
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
        public void SpawnPlayer(Grid grid)
        {
            int index;

            do
            {
                index = UnityEngine.Random.Range(0, grid.EnvironmentLayer.Length);
            }
            while (!_playerSpawnMask.HasFlag(grid.EnvironmentLayer[index].LayerMask));

            PlayerPos = grid.ToV3Int(index);
        }

        /// <summary>
        /// Indique si la case est naviguable par le joueur
        /// </summary>
        /// <param name="layerMask">Les attributs de la case</param>
        public bool TileIsWalkable(EnvironmentTileLayerMask layerMask)
        {
            return _playerWalkableMask.HasFlag(layerMask);
        }

        #endregion
    }
}