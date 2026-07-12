using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.TilePalettes
{
    /// <summary>
    /// Contient les sprites utilisés pour l'affichage des cases
    /// </summary>
    [CreateAssetMenu(fileName = "New Sprite Library", menuName = "Scriptable Objects/Castle of Temptation/Sprite Library")]
    public class SpriteLibrarySO : ScriptableObject
    {
        /// <summary>
        /// Le sprite représentant le joueur
        /// </summary>
        [field: SerializeField]
        public Tile PlayerSprite { get; private set; }
    }
}