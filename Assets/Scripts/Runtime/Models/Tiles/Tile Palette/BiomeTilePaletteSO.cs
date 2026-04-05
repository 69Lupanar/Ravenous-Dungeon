using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Tiles.TilePalette
{
    /// <summary>
    /// Assigne les sprites pour chaque type de case à instancier.
    /// Ca nous permet de créer différents biomes pour un niveau donné
    /// (forêt, château, caverne, etc.)
    /// </summary>
    [CreateAssetMenu(fileName = "New Environment Tile Palette", menuName = "Scriptable Objects/Environment Tile Palette")]
    public class BiomeTilePaletteSO : ScriptableObject
    {
        /// <summary>
        /// Les sprites représentant leurs cases associées
        /// </summary>
        [field: SerializedDictionary("Tile", "Possible Sprites")]
        [field: SerializeField]
        public SerializedDictionary<TileSO, ItemSpawnChance<UnityEngine.Tilemaps.Tile>[]> Tiles { get; private set; }
    }
}