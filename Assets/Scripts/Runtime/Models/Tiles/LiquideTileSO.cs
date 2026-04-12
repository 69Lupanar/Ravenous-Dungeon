using Assets.Scripts.Runtime.Models.Actors;
using Assets.Scripts.Runtime.Models.ValueTypes;
using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Tiles
{
    /// <summary>
    /// Données d'une case représentant une région liquide l'environnement (eau, lave etc.)
    /// </summary>
    [CreateAssetMenu(fileName = "New Environment Tile", menuName = "Scriptable Objects/Tiles/Liquid Tile")]
    public sealed class LiquidTileSO : TileEntitySO, IEnvironmentActor, ILiquidActor
    {
        [field: Space(10)]
        [field: Header("Environment")]
        [field: Space(10)]

        /// <<summary>
        /// Les attributs de cette case
        /// </summary>
        [field: SerializeField]
        public EnvironmentTileLayerMask LayerMask { get; set; }

        /// <summary>
        /// La force du courant. Si un personnage remonte le courant,
        /// son temps de mouvement est multiplié par cette valeur.
        /// S'il descent le courant, il est divisé par cette valeur.
        /// </summary>
        [field: SerializeField]
        public int CurrentStrength { get; set; }
    }
}