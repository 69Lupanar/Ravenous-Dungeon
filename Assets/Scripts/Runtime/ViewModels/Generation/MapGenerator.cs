using System;
using Assets.Scripts.Runtime.Models.Generation;
using Assets.Scripts.Runtime.Models.Tiles.TilePalette;
using Unity.Mathematics;
using UnityEngine;
using Grid = Assets.Scripts.Runtime.Models.Tiles.Map.Grid;
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
            GenerationAlgorithmType[] algos = _generationSettings.AlgorithmsPerBiome[biomePalette];
            GenerationAlgorithmType selectedAlg = algos[Random.Range(0, algos.Length)];

            Generate(_generationSettings, _tileLibrary, selectedAlg, biomePalette);
        }

        /// <summary>
        /// Génère une nouvelle carte
        /// </summary>
        /// <param name="gs">Paramètres de génération</param>
        /// <param name="tl">Contient les cases utilisés pour la génération</param>
        /// <param name="alg">Algorithme de génération sélectionné</param>
        /// <param name="palette">Contient les sprites utilisés pour l'affichage des cases</param>
        public void Generate(GenerationSettingsSO gs, TileLibrarySO tl, GenerationAlgorithmType alg, BiomeTilePaletteSO palette)
        {
            int2 gridSize = new(Random.Range(gs.MinMaxGridSize.x, gs.MinMaxGridSize.y),
                                Random.Range(gs.MinMaxGridSize.x, gs.MinMaxGridSize.y));

            _grid = new Grid(gridSize);

            // Génère l'environnement

            _grid.EnvironmentLayer = GenerationAlgUtils.GenerateEnvironmnent(gs, tl, alg, gridSize);

            OnGenerationEnded?.Invoke(this, new GenerationEndedEventArgs(_grid, palette));
        }

        #endregion
    }
}