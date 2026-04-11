using Assets.Scripts.Runtime.Models.Player;
using UnityEngine;

namespace Assets.Scripts.Runtime.ViewModels.Player
{

    /// <summary>
    /// Contrôle le mouvement de la caméra
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// Décalage de la cible
        /// </summary>
        [SerializeField]
        private Vector3 _offset;

        /// <summary>
        /// Le contrôleur du joueur
        /// </summary>
        [SerializeField]
        private PlayerController _playerController;

        /// <summary>
        /// La cible de la caméra
        /// </summary>
        [SerializeField]
        private Transform _playerCameraFollowTarget;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Start()
        {
            _playerController.OnPlayerSpawned += OnPlayerSpawned;
            _playerController.OnPlayerMoved += OnPlayerMoved;
        }

        /// <summary>
        /// nettoyage
        /// </summary>
        private void OnDestroy()
        {
            _playerController.OnPlayerSpawned -= OnPlayerSpawned;
            _playerController.OnPlayerMoved -= OnPlayerMoved;
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Appelé quand le personnage est placé sur la carte
        /// </summary>
        private void OnPlayerSpawned(object _, PlayerSpawnedEventArgs e)
        {
            SetCameraPosition(e.PlayerPosition);
        }

        /// <summary>
        /// Appelé quand le joueur déplace le personnage
        /// </summary>
        private void OnPlayerMoved(object _, PlayerMovedEventArgs e)
        {
            SetCameraPosition(e.NewPos);
        }

        /// <summary>
        /// Place la caméra à la position renseignée
        /// </summary>
        /// <param name="newPos">Nouvelle position</param>
        private void SetCameraPosition(Vector3Int newPos)
        {
            _playerCameraFollowTarget.position = new Vector3(newPos.x, newPos.y, newPos.z) + _offset;
        }

        #endregion
    }
}