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
        public EnvironmentTileSO GroundTile { get; private set; }

        /// <summary>
        /// La case représentant les zones encore non explorées
        /// </summary>
        [field: SerializeField]
        public EnvironmentTileSO UnknownTile { get; private set; }

        /// <summary>
        /// Les cases représentant les liquides
        /// </summary>
        [field: SerializeField]
        public SerializedDictionary<RiverType, EnvironmentTileSO> RiverTiles { get; private set; }

        /// <summary>
        /// La case représentant le mur
        /// </summary>
        [field: SerializeField]
        public ItemSelectionChance<EnvironmentTileSO>[] WallTiles { get; private set; }


        /// <summary>
        /// La case représentant les portes
        /// </summary>
        [field: SerializeField]
        public ItemSelectionChance<FeatureTileSO>[] DoorTiles { get; private set; }
    }
}