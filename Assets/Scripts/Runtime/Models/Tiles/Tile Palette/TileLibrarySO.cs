using Assets.Scripts.Runtime.Models.ValueTypes;
using AYellowpaper.SerializedCollections;
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
        public StaticEnvironmentTileSO GroundTile { get; private set; }

        /// <summary>
        /// La case représentant les zones encore non explorées
        /// </summary>
        [field: SerializeField]
        public TileEntitySO UnknownTile { get; private set; }

        /// <summary>
        /// Les cases représentant les liquides
        /// </summary>
        [field: SerializeField]
        [field: SerializedDictionary("Tile", "Possible Sprites")]
        public SerializedDictionary<LiquidType, ItemSelectionChance<LiquidTileSO>[]> RiverTiles { get; private set; }

        /// <summary>
        /// La case représentant le mur
        /// </summary>
        [field: SerializeField]
        public ItemSelectionChance<StaticEnvironmentTileSO>[] WallTiles { get; private set; }


        /// <summary>
        /// La case représentant les portes
        /// </summary>
        [field: SerializeField]
        public ItemSelectionChance<DoorTileSO>[] DoorTiles { get; private set; }


        /// <summary>
        /// La case représentant les éléments interactifs
        /// </summary>
        [field: SerializeField]
        public ItemSelectionChance<InteractableTileSO>[] InteractableTiles { get; private set; }
    }
}