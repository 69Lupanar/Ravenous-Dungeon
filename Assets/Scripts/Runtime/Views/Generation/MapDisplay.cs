using Assets.Scripts.Runtime.Models.Generation;
using Assets.Scripts.Runtime.Models.Player;
using Assets.Scripts.Runtime.Models.Tiles;
using Assets.Scripts.Runtime.Models.Tiles.TilePalette;
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
        [field: SerializeField]
        public MapGenerator _mapGenerator { get; private set; }

        /// <summary>
        /// Le PlayerController
        /// </summary>
        [field: SerializeField]
        public PlayerController _playerController { get; private set; }

        /// <summary>
        /// La couche de la tilemap contenant les cases de l'environnement
        /// </summary>
        [field: SerializeField]
        public Tilemap _environmentTilemap { get; private set; }

        /// <summary>
        /// La couche de la tilemap contenant le joueur
        /// </summary>
        [field: SerializeField]
        public Tilemap _playerTilemap { get; private set; }

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
            _playerController.OnPlayerMoved += OnPlayerMoved;
        }

        /// <summary>
        /// Nettoyage
        /// </summary>
        private void OnDestroy()
        {
            _mapGenerator.OnGenerationEnded -= OnGenerationEnded;
            _playerController.OnPlayerMoved -= OnPlayerMoved;
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
            _curPalette = e.SpriteLibrary;
            DisplayEnvironment(e.Grid.GridSize, e.Grid.EnvironmentLayer, e.SpriteLibrary);
            DisplayPlayer(Vector3Int.zero, _playerController.PlayerPos, e.SpriteLibrary);
        }

        /// <summary>
        /// Appelée quand le joueur déplace son personnage
        /// </summary>
        /// <param name="e">Les infos sur l'action</param>
        private void OnPlayerMoved(object _, PlayerMovedEventArgs e)
        {
            DisplayPlayer(e.PreviousPos, e.NewPos, _curPalette);
        }

        /// <summary>
        /// Efface la carte
        /// </summary>
        private void Clear()
        {
            _environmentTilemap.ClearAllTiles();
            _playerTilemap.ClearAllTiles();
        }

        /// <summary>
        /// Affiche la carte à l'écran
        /// </summary>
        /// <param name="gridSize">Les dimensions de la grille</param>
        /// <param name="layer">La couche à afficher</param>
        /// <param name="sl">Contient les sprites utilisés pour l'affichage des cases</param>
        private void DisplayEnvironment(int2 gridSize, TileEntitySO[] layer, SpriteLibrarySO sl)
        {
            for (int i = 0; i < layer.Length; ++i)
            {
                int2 xy = new(i % gridSize.x, i / gridSize.x);
                TileEntitySO tile = layer[i];
                ItemSelectionChance<Tile>[] possibleTiles = sl.Tiles[tile];
                Tile selectedTile = possibleTiles.Sample();

                _environmentTilemap.SetTile(new Vector3Int(xy.x, xy.y), selectedTile);
            }
        }

        /// <summary>
        /// Affiche le joueur sur la carte
        /// </summary>
        /// <param name="previousPos">La position précédente du joueur</param>
        /// <param name="curPos">La position actuelle du joueur</param>
        /// <param name="sl">Contient les sprites utilisés pour l'affichage des cases</param>
        private void DisplayPlayer(Vector3Int previousPos, Vector3Int curPos, SpriteLibrarySO sl)
        {
            _playerTilemap.SetTile(previousPos, null);
            _playerTilemap.SetTile(curPos, sl.PlayerSprite);
        }

        #endregion
    }
}