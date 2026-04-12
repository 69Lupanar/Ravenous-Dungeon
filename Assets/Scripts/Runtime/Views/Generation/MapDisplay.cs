using Assets.Scripts.Runtime.Models.Actors;
using Assets.Scripts.Runtime.Models.Generation;
using Assets.Scripts.Runtime.Models.Player;
using Assets.Scripts.Runtime.Models.Tiles.TilePalette;
using Assets.Scripts.Runtime.Models.ValueTypes;
using Assets.Scripts.Runtime.ViewModels.Extensions;
using Assets.Scripts.Runtime.ViewModels.Generation;
using Assets.Scripts.Runtime.ViewModels.Player;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

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
        [SerializeField]
        private MapGenerator _mapGenerator;

        /// <summary>
        /// Le PlayerController
        /// </summary>
        [SerializeField]
        private PlayerController _playerController;

        /// <summary>
        /// La couche de la tilemap contenant les cases de l'environnement
        /// </summary>
        [SerializeField]
        private Tilemap _environmentTilemap;

        /// <summary>
        /// La couche de la tilemap contenant les cases interagissables
        /// </summary>
        [SerializeField]
        private Tilemap _interactablesTilemap;

        /// <summary>
        /// La couche de la tilemap contenant le joueur
        /// </summary>
        [SerializeField]
        private Tilemap _playerTilemap;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// La palette du niveau actuel
        /// </summary>
        private SpriteLibrarySO _curPalette;

        #endregion

        #region Méthodes Unity

        /// <summary>
        /// init
        /// </summary>
        private void Awake()
        {
            _mapGenerator.OnGenerationEnded += OnGenerationEnded;
            _playerController.OnPlayerSpawned += OnPlayerSpawned;
            _playerController.OnPlayerMoved += OnPlayerMoved;
        }

        /// <summary>
        /// Nettoyage
        /// </summary>
        private void OnDestroy()
        {
            _mapGenerator.OnGenerationEnded -= OnGenerationEnded;
            _playerController.OnPlayerSpawned -= OnPlayerSpawned;
            _playerController.OnPlayerMoved -= OnPlayerMoved;
        }

        #endregion

        #region Méthodes privées

        #region Callbacks

        /// <summary>
        /// Appelé quand la génération est terminée
        /// </summary>
        /// <param name="e">Les infos de la génération</param>
        private void OnGenerationEnded(object _, GenerationEndedEventArgs e)
        {
            Clear();
            _curPalette = e.SpriteLibrary;
            DisplayEnvironment(e.Grid.GridSize, e.Grid.StaticEnvironmentLayer, e.SpriteLibrary, _environmentTilemap);
            DisplayLiquids(e.Grid.GridSize, e.Grid.LiquidsLayer, e.SpriteLibrary, _environmentTilemap);
            DisplayDoors(e.Grid.GridSize, e.Grid.DoorsLayer, e.SpriteLibrary, _interactablesTilemap);
            DisplayInteractables(e.Grid.GridSize, e.Grid.InteractablesLayer, e.SpriteLibrary, _interactablesTilemap);
        }

        private void OnPlayerSpawned(object sender, PlayerSpawnedEventArgs e)
        {
            DisplayPlayer(Vector3Int.zero, _playerController.PlayerPos, _curPalette, _playerTilemap);
        }

        /// <summary>
        /// Appelée quand le joueur déplace son personnage
        /// </summary>
        /// <param name="e">Les infos sur l'action</param>
        private void OnPlayerMoved(object _, PlayerMovedEventArgs e)
        {
            DisplayPlayer(e.PreviousPos, e.NewPos, _curPalette, _playerTilemap);
        }

        #endregion

        /// <summary>
        /// Efface la carte
        /// </summary>
        private void Clear()
        {
            _environmentTilemap.ClearAllTiles();
            _interactablesTilemap.ClearAllTiles();
            _playerTilemap.ClearAllTiles();
        }

        /// <summary>
        /// Affiche la carte à l'écran
        /// </summary>
        /// <param name="gridSize">Les dimensions de la grille</param>
        /// <param name="layer">La couche à afficher</param>
        /// <param name="sl">Contient les sprites utilisés pour l'affichage des cases</param>
        /// <param name="tilemap">La couche de la grille</param>
        private void DisplayEnvironment(int2 gridSize, StaticEnvironmentActor[] layer, SpriteLibrarySO sl, Tilemap tilemap)
        {
            for (int i = 0; i < layer.Length; ++i)
            {
                int2 xy = new(i % gridSize.x, i / gridSize.x);
                StaticEnvironmentActor tile = layer[i];

                if (tile.Data != null)
                {
                    ItemSelectionChance<Tile>[] possibleTiles = sl.StaticEnvironmentTiles[tile.Data];
                    Tile selectedTile = possibleTiles.Sample();

                    tilemap.SetTile(new Vector3Int(xy.x, xy.y), selectedTile);
                }
            }
        }


        /// <summary>
        /// Affiche la carte à l'écran
        /// </summary>
        /// <param name="gridSize">Les dimensions de la grille</param>
        /// <param name="layer">La couche à afficher</param>
        /// <param name="sl">Contient les sprites utilisés pour l'affichage des cases</param>
        /// <param name="tilemap">La couche de la grille</param>
        private void DisplayLiquids(int2 gridSize, LiquidActor[] layer, SpriteLibrarySO sl, Tilemap tilemap)
        {
            for (int i = 0; i < layer.Length; ++i)
            {
                int2 xy = new(i % gridSize.x, i / gridSize.x);
                LiquidActor tile = layer[i];

                if (tile.Data != null)
                {
                    ItemSelectionChance<Tile>[] possibleTiles = sl.LiquidTiles[tile.Data];
                    Tile selectedTile = possibleTiles.Sample();

                    tilemap.SetTile(new Vector3Int(xy.x, xy.y), selectedTile);
                }
            }
        }

        /// <summary>
        /// Affiche les cases spéciales
        /// </summary>
        /// <param name="gridSize">Les dimensions de la grille</param>
        /// <param name="layer">La couche à afficher</param>
        /// <param name="sl">Contient les sprites utilisés pour l'affichage des cases</param>
        /// <param name="tilemap">La couche de la grille</param>
        private void DisplayDoors(int2 gridSize, DoorActor[] layer, SpriteLibrarySO sl, Tilemap tilemap)
        {
            for (int i = 0; i < layer.Length; ++i)
            {
                int2 xy = new(i % gridSize.x, i / gridSize.x);
                DoorActor tile = layer[i];

                if (tile.Data != null)
                {
                    ItemSelectionChance<Tile>[] possibleTiles = sl.DoorTiles[tile.Data];
                    Tile selectedTile = possibleTiles.Sample();

                    tilemap.SetTile(new Vector3Int(xy.x, xy.y), selectedTile);
                }
            }
        }

        /// <summary>
        /// Affiche les cases spéciales
        /// </summary>
        /// <param name="gridSize">Les dimensions de la grille</param>
        /// <param name="layer">La couche à afficher</param>
        /// <param name="sl">Contient les sprites utilisés pour l'affichage des cases</param>
        /// <param name="tilemap">La couche de la grille</param>
        private void DisplayInteractables(int2 gridSize, InteractableActor[] layer, SpriteLibrarySO sl, Tilemap tilemap)
        {
            for (int i = 0; i < layer.Length; ++i)
            {
                int2 xy = new(i % gridSize.x, i / gridSize.x);
                InteractableActor tile = layer[i];

                if (tile.Data != null)
                {
                    ItemSelectionChance<Tile>[] possibleTiles = sl.IneractableTiles[tile.Data];
                    Tile selectedTile = possibleTiles.Sample();

                    tilemap.SetTile(new Vector3Int(xy.x, xy.y), selectedTile);
                }
            }
        }

        /// <summary>
        /// Affiche le joueur sur la carte
        /// </summary>
        /// <param name="previousPos">La position précédente du joueur</param>
        /// <param name="curPos">La position actuelle du joueur</param>
        /// <param name="sl">Contient les sprites utilisés pour l'affichage des cases</param>
        /// <param name="tilemap">La couche de la grille</param>
        private void DisplayPlayer(Vector3Int previousPos, Vector3Int curPos, SpriteLibrarySO sl, Tilemap tilemap)
        {
            tilemap.SetTile(previousPos, null);
            tilemap.SetTile(curPos, sl.PlayerSprite);
        }

        #endregion
    }
}