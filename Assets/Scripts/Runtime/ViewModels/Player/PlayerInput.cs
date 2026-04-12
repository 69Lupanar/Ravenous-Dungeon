using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Runtime.ViewModels.Player
{
    /// <summary>
    /// Obtient les inputs du joueur
    /// </summary>
    public class PlayerInput : MonoBehaviour
    {
        #region Propriétés

        /// <summary>
        /// Direction de mouvement
        /// </summary>
        internal int2 MoveDirection { get; set; }

        /// <summary>
        /// Direction de mouvement
        /// </summary>
        internal bool RequestedMoveThisFrame => math.any(MoveDirection != int2.zero);

        #endregion

        #region Variables Unity

        /// <summary>
        /// true si le déplacement peut se faire dans 8 direction, false pour 4 seulement
        /// </summary>
        [SerializeField]
        private bool _allow8Way = true;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// La table des inputs
        /// </summary>
        private PlayerInputActions _input;

        #endregion

        #region Méthodes Unity

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _input = new PlayerInputActions();
            _input.Player.Enable();

            _input.Player.MoveUp.started += ctx => MoveDirection = new int2(0, 1);
            _input.Player.MoveRight.started += ctx => MoveDirection = new int2(1, 0);
            _input.Player.MoveBottom.started += ctx => MoveDirection = new int2(0, -1);
            _input.Player.MoveLeft.started += ctx => MoveDirection = new int2(-1, 0);

            if (_allow8Way)
            {
                _input.Player.MoveUpRight.started += ctx => MoveDirection = new int2(1, 1);
                _input.Player.MoveBottomRight.started += ctx => MoveDirection = new int2(1, -1);
                _input.Player.MoveBottomLeft.started += ctx => MoveDirection = new int2(-1, -1);
                _input.Player.MoveUpLeft.started += ctx => MoveDirection = new int2(-1, 1);
            }
        }

        /// <summary>
        /// màj après chaque frame
        /// </summary>
        private void LateUpdate()
        {
            MoveDirection = int2.zero;
        }

        /// <summary>
        /// nettoyage
        /// </summary>
        private void OnDestroy()
        {
            _input.Disable();
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Active ou désacive les contrôles du joueur
        /// </summary>
        public void EnablePlayerMap(bool enabled)
        {
            if (enabled)
            {
                _input.Player.Enable();
            }
            else
            {
                _input.Player.Disable();
            }
        }

        /// <summary>
        /// Active ou désacive les contrôles de l'UI
        /// </summary>
        public void EnableUIMap(bool enabled)
        {
            if (enabled)
            {
                _input.UI.Enable();
            }
            else
            {
                _input.UI.Disable();
            }
        }

        #endregion
    }
}