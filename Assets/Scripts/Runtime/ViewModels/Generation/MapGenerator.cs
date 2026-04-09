using System;
using Assets.Scripts.Runtime.Models.Generation;
using Assets.Scripts.Runtime.Models.Tiles.TilePalette;
using Assets.Scripts.Runtime.ViewModels.Generation.Algorithms;
using Assets.Scripts.Runtime.ViewModels.Player;
using Unity.Mathematics;
using UnityEngine;
using Grid = Assets.Scripts.Runtime.Models.Map.Grid;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Runtime.ViewModels.Generation
{
    /// <summary>
    /// Génère une nouvelle carte
    /// </summary>
    public class MapGenerator : MonoBehaviour
    {
        #region Evénements

        /// <summary>
        /// Appelé quand la génération est terminée
        /// </summary>
        public EventHandler<GenerationEndedEventArgs> OnGenerationEnded;

        #endregion

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
        /// Le PlayerController
        /// </summary>
        [field: SerializeField]
        public PlayerController _playerController { get; private set; }

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
            BiomeTilePaletteSO biomePalette = _generationSettings.Biomes[Random.Range(0, _generationSettings.Biomes.Length)];
            GenerationAlgorithmSettingsSO[] algos = _generationSettings.AlgorithmsPerBiome[biomePalette];
            GenerationAlgorithmSettingsSO selectedAlg = algos[Random.Range(0, algos.Length)];

            Generate(_generationSettings, _tileLibrary, selectedAlg, biomePalette);
        }

        /// <summary>
        /// Génère une nouvelle carte
        /// </summary>
        /// <param name="gs">Paramètres de génération</param>
        /// <param name="tl">Contient les cases utilisés pour la génération</param>
        /// <param name="alg">Algorithme de génération sélectionné</param>
        /// <param name="palette">Contient les sprites utilisés pour l'affichage des cases</param>
        public void Generate(GenerationSettingsSO gs, TileLibrarySO tl, GenerationAlgorithmSettingsSO alg, BiomeTilePaletteSO palette)
        {
            int2 gridSize = new(Random.Range(gs.MinMaxGridSize.x, gs.MinMaxGridSize.y),
                                Random.Range(gs.MinMaxGridSize.x, gs.MinMaxGridSize.y));

            _grid = new Grid(gridSize);

            // Génère l'environnement

            _grid.EnvironmentLayer = GenerationAlgUtils.GenerateEnvironmnent(tl, alg, gridSize);

            // Place le joueur sur la carte

            _playerController.SetGrid(_grid);
            _playerController.SpawnPlayer(_grid);

            OnGenerationEnded?.Invoke(this, new GenerationEndedEventArgs(_grid, palette));
        }

        #endregion
    }
}