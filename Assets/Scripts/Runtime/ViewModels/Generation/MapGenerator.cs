using Assets.Scripts.Runtime.Models.Tiles;
using Assets.Scripts.Runtime.Models.Tiles.Generation;
using Assets.Scripts.Runtime.Models.Tiles.TilePalette;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Grid = Assets.Scripts.Runtime.Models.Tiles.Map.Grid;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Runtime.ViewModels.Generation
{
    /// <summary>
    /// Génère une nouvelle carte
    /// </summary>
    public class MapGenerator : MonoBehaviour
    {
        #region Variables Unity

        /// <summary>
        /// Paramètres de génération
        /// </summary>
        [field: SerializeField]
        public GenerationSettingsSO _generationSettings { get; private set; }

        /// <summary>
        /// Contient les cases utilisés pour la génération
        /// </summary>
        [field: SerializeField]
        public TileLibrarySO _tileLibrary { get; private set; }

        /// <summary>
        /// Contient les sprites utilisés pour l'affichage des cases
        /// </summary>
        [field: SerializeField]
        public SpriteLibrarySO _spriteLibrary { get; private set; }

        /// <summary>
        /// La couche de la tilemap contenant les cases de l'environnement
        /// </summary>
        [field: SerializeField]
        public Tilemap _environmentTileMap { get; private set; }

        #endregion

        #region Variables d'instance

        /// <summary>
        /// Grille contenant les cases
        /// </summary>
        private Grid _grid;

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Génère une nouvelle carte
        /// </summary>
        [ContextMenu("Generate")]
        public void Generate()
        {
            Clear();
            _grid = Generate(_generationSettings, _tileLibrary);
            Display(_grid, _spriteLibrary);
        }

        /// <summary>
        /// Génère une nouvelle carte
        /// </summary>
        /// <param name="gs">Paramètres de génération</param>
        /// <param name="tileLibrary">Contient les cases utilisés pour la génération</param>
        public Grid Generate(GenerationSettingsSO gs, TileLibrarySO tileLibrary)
        {
            int2 gridSize = new(Random.Range(gs.MinMaxGridSize.x, gs.MinMaxGridSize.y),
                                Random.Range(gs.MinMaxGridSize.x, gs.MinMaxGridSize.y));

            TileSO[] environmentLayer = new TileSO[gridSize.x * gridSize.y];

            // Pour l'instant on fait juste une salle recouvrant toute la carte

            int count = 0;

            for (int i = 0; i < gridSize.x; ++i)
            {
                environmentLayer[i] = tileLibrary.WallTile;
            }

            count += gridSize.x;

            for (int i = 1; i < gridSize.y - 1; ++i)
            {
                environmentLayer[count] = tileLibrary.WallTile;

                for (int j = 1; j < gridSize.x; ++j)
                {
                    environmentLayer[count + j] = tileLibrary.GroundTile;
                }

                environmentLayer[count + gridSize.x - 1] = tileLibrary.WallTile;

                count += gridSize.x;
            }

            for (int i = count; i < count + gridSize.x; ++i)
            {
                environmentLayer[i] = tileLibrary.WallTile;
            }

            return new Grid(gridSize, environmentLayer);
        }

        /// <summary>
        /// Affiche la carte à l'écran
        /// </summary>
        /// <param name="grid">La grille des cases créées</param>
        /// <param name="spriteLibrary">Contient les sprites utilisés pour l'affichage des cases</param>
        public void Display(Grid grid, SpriteLibrarySO spriteLibrary)
        {
            BiomeTilePaletteSO biomePalette = spriteLibrary.Palettes[0];

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

        /// <summary>
        /// Efface la carte
        /// </summary>
        [ContextMenu("Clear")]
        private void Clear()
        {
            _environmentTileMap.ClearAllTiles();
        }

        #endregion
    }
}