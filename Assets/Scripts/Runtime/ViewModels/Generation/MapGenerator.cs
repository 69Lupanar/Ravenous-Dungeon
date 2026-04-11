using System;
using Assets.Scripts.Runtime.Models.Generation;
using Assets.Scripts.Runtime.ViewModels.Extensions;
using Assets.Scripts.Runtime.ViewModels.Generation.Algorithms;
using Assets.Scripts.Runtime.ViewModels.Player;
using Unity.Mathematics;
using UnityEngine;
using Grid = Assets.Scripts.Runtime.Models.Map.Grid;

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
        [SerializeField]
        private GenerationSettingsSO[] _generationSettings;

        /// <summary>
        /// Le PlayerController
        /// </summary>
        [SerializeField]
        private PlayerController _playerController;

        /// <summary>
        /// Graine de génération
        /// </summary>
        [field: SerializeField]
        private uint _seed;

        #endregion

        #region Variables d'instance

        /// <summary>
        /// Grille contenant les cases
        /// </summary>
        private Grid _grid;

        /// <summary>
        /// Générateur d'aléatoire
        /// </summary>
        private Unity.Mathematics.Random _rand;

        #endregion

        #region Méthodes Unity

#if UNITY_EDITOR

        /// <summary>
        /// Appelée quand modif dans l'inspecteur
        /// </summary>
        private void OnValidate()
        {
            _rand = new Unity.Mathematics.Random(_seed == 0 ? (uint)UnityEngine.Random.Range(1, uint.MaxValue) : _seed);
        }

        /// <summary>
        /// init
        /// </summary>
        private void Start()
        {
            Generate();
        }

        /// <summary>
        /// màj à chaque frame
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Generate();
            }
        }

#endif

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Génère une nouvelle carte
        /// </summary>
        [ContextMenu("Generate")]
        public void Generate()
        {
            GenerationSettingsSO gs = _generationSettings[_rand.NextInt(_generationSettings.Length)];
            Generate(gs);
        }

        /// <summary>
        /// Génère une nouvelle carte
        /// </summary>
        /// <param name="gs">Paramètres de génération</param>
        public void Generate(GenerationSettingsSO gs)
        {
            int2 gridSize = new(_rand.NextInt(gs.GridSizeInterval.x, gs.GridSizeInterval.y), _rand.NextInt(gs.GridSizeInterval.x, gs.GridSizeInterval.y));
            int nbRiversToGenerate = _rand.NextFloat(100f) < gs.RiverSpawnRate ? _rand.NextInt(gs.NbRiversInterval.x, gs.NbRiversInterval.y) : 0;

            _grid = new Grid(gridSize);
            GenerationAlgorithmSettingsSO selectedAlg = gs.Algorithms.Sample(ref _rand);

            // Génère l'environnement

            GenerationAlgUtils.GenerateEnvironmnent(gs.TileLibrary, selectedAlg, _grid, ref _rand);

            // Génère les rivières

            GenerationAlgUtils.GenerateRivers(gs.TileLibrary, gs.RiverTypes, gs.RiverWidthInterval, nbRiversToGenerate, _grid, ref _rand);

            // Génère les Features

            GenerationAlgUtils.GenerateFeatures(gs.TileLibrary, selectedAlg, _grid, ref _rand);

            // Place le joueur sur la carte

            _playerController.SetGrid(_grid);
            _playerController.SpawnPlayer(_grid, ref _rand);

            OnGenerationEnded?.Invoke(this, new GenerationEndedEventArgs(_grid, gs.SpriteLibrary));
        }

        #endregion
    }
}