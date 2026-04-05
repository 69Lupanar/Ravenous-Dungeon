using Assets.Scripts.Runtime.Models.Tiles;
using Assets.Scripts.Runtime.Models.Tiles.Generation;
using Assets.Scripts.Runtime.Models.Tiles.TilePalette;
using Assets.Scripts.Runtime.ViewModels.Generation;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Runtime.Views.Generation
{
    /// <summary>
    /// Affiche et màj la carte à l'écran
    /// </summary>
    public class MapDisplay : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// Le générateur de carte
        /// </summary>
        [field: SerializeField]
        public MapGenerator _mapGenerator { get; private set; }

        /// <summary>
        /// La couche de la tilemap contenant les cases de l'environnement
        /// </summary>
        [field: SerializeField]
        public Tilemap _environmentTileMap { get; private set; }

        /// <summary>
        /// Contient les sprites utilisés pour l'affichage des cases
        /// </summary>
        [field: SerializeField]
        public SpriteLibrarySO _spriteLibrary { get; private set; }

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Awake()
        {
            _mapGenerator.OnGenerationEnded += OnGenerationEnded;
        }

        /// <summary>
        /// Nettoyage
        /// </summary>
        private void OnDestroy()
        {
            _mapGenerator.OnGenerationEnded -= OnGenerationEnded;
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Efface la carte
        /// </summary>
        [ContextMenu("Clear")]
        public void Clear()
        {
            _environmentTileMap.ClearAllTiles();
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Appelé quand la génération est terminée
        /// </summary>
        /// <param name="e">Les infos de la génération</param>
        private void OnGenerationEnded(object _, GenerationEndedEventArgs e)
        {
            Clear();
            Display(e.Grid, e.BiomePalette);
        }

        /// <summary>
        /// Affiche la carte à l'écran
        /// </summary>
        /// <param name="grid">La grille des cases créées</param>
        /// <param name="biomePalette">Contient les sprites utilisés pour l'affichage des cases</param>
        public void Display(Models.Tiles.Map.Grid grid, BiomeTilePaletteSO biomePalette)
        {
            for (int y = 0; y < grid.GridSize.y; ++y)
            {
                for (int x = 0; x < grid.GridSize.x; ++x)
                {
                    TileSO tile = grid.EnvironmentLayer[grid.ToIndex(x, y)];
                    ItemSpawnChance<Tile>[] spawnChances = biomePalette.Tiles[tile];
                    Tile selectedTile = null;

                    if (spawnChances.Length == 1)
                    {
                        selectedTile = spawnChances[0].Value;
                    }
                    else
                    {
                        float rand = Random.Range(0f, 100f);
                        float2 curInterval = new(0f, 0f);

                        foreach (ItemSpawnChance<Tile> item in spawnChances)
                        {
                            curInterval.y += item.Chance;

                            if (curInterval.x < rand && rand < curInterval.y)
                            {
                                selectedTile = item.Value;
                                break;
                            }

                            curInterval.x += item.Chance;
                        }
                    }

                    _environmentTileMap.SetTile(new Vector3Int(x, y), selectedTile);
                }
            }
        }

        #endregion
    }
}