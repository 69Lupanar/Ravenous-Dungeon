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
        /// Les palettes de sprite pour les différents biomes du jeu
        /// </summary>
        [field: SerializeField]
        public BiomeTilePaletteSO[] Palettes { get; private set; }
    }
}