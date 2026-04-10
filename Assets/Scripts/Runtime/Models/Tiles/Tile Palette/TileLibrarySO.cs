using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Tiles.TilePalette
{
    /// <summary>
    /// Contient les cases utilisés pour la génération
    /// </summary>
    [CreateAssetMenu(fileName = "New Tile Library", menuName = "Scriptable Objects/Tile Library")]
    public class TileLibrarySO : ScriptableObject
    {
        /// <summary>
        /// La case représentant le sol
        /// </summary>
        [field: SerializeField]
        public TileEntitySO GroundTile { get; private set; }

        /// <summary>
        /// La case représentant les zones encore non explorées
        /// </summary>
        [field: SerializeField]
        public TileEntitySO UnknownTile { get; private set; }

        /// <summary>
        /// La case représentant le mur
        /// </summary>
        [field: SerializeField]
        public ItemSelectionChance<TileEntitySO>[] WallTiles { get; private set; }


        /// <summary>
        /// La case représentant les portes
        /// </summary>
        [field: SerializeField]
        public ItemSelectionChance<TileEntitySO>[] DoorTiles { get; private set; }
    }
}