using Assets.Scripts.Runtime.Models.Tiles;
using UnityEngine;
using Grid = Assets.Scripts.Runtime.Models.Map.Grid;

namespace Assets.Scripts.Runtime.ViewModels.Player
{
    /// <summary>
    /// Chargé d'instancier et contrôler le joueur
    /// après la génération d'un niveau
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        #region Propriétés

        /// <summary>
        /// La position du joueur
        /// </summary>
        public Vector3Int PlayerPos { get; private set; }

        #endregion

        #region Variables Unity

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
            int index = 0;

            do
            {
                index = Random.Range(0, grid.EnvironmentLayer.Length);
            }
            while (!_playerSpawnMask.HasFlag(grid.EnvironmentLayer[index].LayerMask));

            PlayerPos = grid.ToV3Int(index);
        }

        #endregion
    }
}