using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Tiles.TilePalette
{
    /// <summary>
    /// Contient les cases utilisés pour la génération
    /// </summary>
    [CreateAssetMenu(fileName = "New Tile Library", menuName = "Scriptable Objects/Generation/Tile Library")]
    public class TileLibrarySO : ScriptableObject
    {
        /// <summary>
        /// La case représentant le joueur
        /// </summary>
        [field: SerializeField]
        public TileSO PlayerTile { get; private set; }

        /// <summary>
        /// La case représentant le mur
        /// </summary>
        [field: SerializeField]
        public TileSO WallTile { get; private set; }

        /// <summary>
        /// La case représentant le sol
        /// </summary>
        [field: SerializeField]
        public TileSO GroundTile { get; private set; }

        /// <summary>
        /// La case représentant les zones encore non explorées
        /// </summary>
        [field: SerializeField]
        public TileSO UnknownTile { get; private set; }
    }
}