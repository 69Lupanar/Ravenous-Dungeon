using Assets.Scripts.Runtime.Models.ValueTypes;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Tiles.TilePalette
{
    /// <summary>
    /// Contient les sprites utilisés pour l'affichage des cases
    /// </summary>
    [CreateAssetMenu(fileName = "New Sprite Library", menuName = "Scriptable Objects/Sprite Library")]
    public class SpriteLibrarySO : ScriptableObject
    {
        /// <summary>
        /// Le sprite représentant le joueur
        /// </summary>
        [field: SerializeField]
        public UnityEngine.Tilemaps.Tile PlayerSprite { get; private set; }

        /// <summary>
        /// Les sprites représentant leurs cases associées
        /// </summary>
        [field: SerializedDictionary("Tile", "Possible Sprites")]
        [field: SerializeField]
        public SerializedDictionary<StaticEnvironmentTileSO, ItemSelectionChance<UnityEngine.Tilemaps.Tile>[]> StaticEnvironmentTiles { get; private set; }

        /// <summary>
        /// Les sprites représentant leurs cases associées
        /// </summary>
        [field: SerializedDictionary("Tile", "Possible Sprites")]
        [field: SerializeField]
        public SerializedDictionary<LiquidTileSO, ItemSelectionChance<UnityEngine.Tilemaps.Tile>[]> LiquidTiles { get; private set; }

        /// <summary>
        /// Les sprites représentant leurs cases associées
        /// </summary>
        [field: SerializedDictionary("Tile", "Possible Sprites")]
        [field: SerializeField]
        public SerializedDictionary<DoorTileSO, ItemSelectionChance<UnityEngine.Tilemaps.Tile>[]> DoorTiles { get; private set; }

        /// <summary>
        /// Les sprites représentant leurs cases associées
        /// </summary>
        [field: SerializedDictionary("Tile", "Possible Sprites")]
        [field: SerializeField]
        public SerializedDictionary<InteractableTileSO, ItemSelectionChance<UnityEngine.Tilemaps.Tile>[]> IneractableTiles { get; private set; }
    }
}