using System;
using Assets.Scripts.Runtime.Models.Generation;
using Assets.Scripts.Runtime.ViewModels.Extensions;
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
        /// Liste de paramètres de génération possibles
        /// </summary>
        [field: SerializeField]
        public GenerationSettingsSO[] _generationettings { get; private set; }

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
            GenerationSettingsSO gs = _generationettings[Random.Range(0, _generationettings.Length)];
            Generate(gs);
        }

        /// <summary>
        /// Génère une nouvelle carte
        /// </summary>
        /// <param name="gs">Paramètres de génération</param>
        /// <param name="alg">Algorithme de génération sélectionné</param>
        public void Generate(GenerationSettingsSO gs)
        {
            int2 gridSize = new(Random.Range(gs.MinMaxGridSize.x, gs.MinMaxGridSize.y),
                                Random.Range(gs.MinMaxGridSize.x, gs.MinMaxGridSize.y));

            _grid = new Grid(gridSize);
            GenerationAlgorithmSettingsSO selectedAlg = gs.Algorithms.Sample();

            // Génère l'environnement

            GenerationAlgUtils.GenerateEnvironmnent(gs.TileLibrary, selectedAlg, _grid);

            // Génère les Features

            GenerationAlgUtils.GenerateFeatures(gs.TileLibrary, selectedAlg, _grid);

            // Place le joueur sur la carte

            _playerController.SetGrid(_grid);
            _playerController.SpawnPlayer(_grid);

            OnGenerationEnded?.Invoke(this, new GenerationEndedEventArgs(_grid, gs.SpriteLibrary));
        }

        #endregion
    }
}