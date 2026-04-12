using Assets.Scripts.Runtime.Models.Actors;
using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Tiles
{
    /// <summary>
    /// Données d'une case représentant un élément interactif
    /// </summary>
    [CreateAssetMenu(fileName = "New Interactable Tile", menuName = "Scriptable Objects/Tiles/Interactable Tile")]
    public sealed class InteractableTileSO : TileEntitySO, IInteractableActor
    {
        /// <summary>
        /// Nombre maximum d'utilisations possibles
        /// </summary>
        [field: SerializeField]
        public int NbMaxUses { get; set; }
    }
}