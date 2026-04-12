using System;
using Assets.Scripts.Runtime.Models.Actors;
using Assets.Scripts.Runtime.Models.Generation;
using Assets.Scripts.Runtime.Models.Generation.Algorithms;
using Assets.Scripts.Runtime.Models.Tiles;
using Assets.Scripts.Runtime.Models.Tiles.TilePalette;
using Assets.Scripts.Runtime.Models.ValueTypes;
using Assets.Scripts.Runtime.ViewModels.Extensions;
using Assets.Scripts.Runtime.ViewModels.Generation.Algorithms;
using Assets.Scripts.Runtime.ViewModels.Player;
using Unity.Mathematics;
using UnityEngine;
using Grid = Assets.Scripts.Runtime.Models.Map.Grid;
using Random = Unity.Mathematics.Random;

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

            _grid = new Grid(gridSize);
            GenerationAlgorithmSettingsSO selectedAlg = gs.Algorithms.Sample(ref _rand);
            RiverGenerationSettingsSO selectedRiverSettings = gs.RiverGenerationSettings.Sample(ref _rand);

            // Génère l'environnement

            GenerateEnvironmnent(gs.TileLibrary, selectedAlg, _grid, ref _rand);

            // Génère les rivières

            GenerateRivers(gs.TileLibrary, selectedRiverSettings, _grid, ref _rand);

            // Génère les éléments interactifs

            GenerateInteractables(gs.TileLibrary, selectedAlg, _grid, ref _rand);

            OnGenerationEnded?.Invoke(this, new GenerationEndedEventArgs(_grid, gs.SpriteLibrary));

            // Place le joueur sur la carte

            _playerController.SetGrid(_grid);
            _playerController.SpawnPlayer(_grid, ref _rand);
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Génère une nouvelle carte
        /// </summary>
        /// <param name="tl">Contient les cases utilisés pour la génération</param>
        /// <param name="alg">Algorithme de génération sélectionné</param>
        /// <param name="grid">La grille</param>
        /// <param name="rand">Générateur d'aléatoire</param>
        private void GenerateEnvironmnent(TileLibrarySO tl, GenerationAlgorithmSettingsSO alg, Grid grid, ref Random rand)
        {
            switch (alg)
            {
                case OneRoomAlgorithmSettingsSO:
                    OneRoomAlg.GenerateEnvironmnent(tl, grid);
                    break;
                case RoomsAndCorridorsAlgorithmSettingsSO settings:
                    RoomsAndCorridorsAlg.GenerateEnvironmnent(settings, tl, grid, ref rand);
                    break;
            }
        }

        /// <summary>
        /// Génère une nouvelle carte
        /// </summary>
        /// <param name="tl">Contient les cases utilisés pour la génération</param>
        /// <param name="alg">Algorithme de génération sélectionné</param>
        /// <param name="grid">La grille</param>
        /// <param name="rand">Générateur d'aléatoire</param>
        private void GenerateInteractables(TileLibrarySO tl, GenerationAlgorithmSettingsSO alg, Grid grid, ref Random rand)
        {
            switch (alg)
            {
                case OneRoomAlgorithmSettingsSO:

                    break;

                case RoomsAndCorridorsAlgorithmSettingsSO settings:

                    GenerateDoors(tl.DoorTiles, settings.DoorSpawnRate, grid, ref rand);
                    break;
            }
        }

        /// <summary>
        /// Génère des portes aux entrées des salles
        /// </summary>
        /// <param name="doorTiles">Liste des cases possibles pour représenter les portes</param>
        /// <param name="doorSpawnRate">%age de chance de créer une porte à une position donnée</param>
        /// <param name="grid">La grille</param>
        /// <param name="rand">Générateur d'aléatoire</param>
        private static void GenerateDoors(ItemSelectionChance<DoorTileSO>[] doorTiles, int doorSpawnRate, Grid grid, ref Random rand)
        {
            foreach (DungeonStructure room in grid.Rooms)
            {
                Span<int2> borderCoords = stackalloc int2[(room.Position.x + room.Dimensions.x) * 2 + (room.Position.y + room.Dimensions.y) * 2];
                int cur = 0;

                // Mur du bas et du haut

                for (int i = room.Position.x; i < room.Position.x + room.Dimensions.x; ++i)
                {
                    borderCoords[cur] = new int2(i, room.Position.y - 1);
                    ++cur;
                    borderCoords[cur] = new int2(i, room.Position.y + room.Dimensions.y);
                    ++cur;
                }

                // Mur de gauche et de droite

                for (int i = room.Position.y; i < room.Position.y + room.Dimensions.y; ++i)
                {
                    borderCoords[cur] = new int2(room.Position.x - 1, i);
                    ++cur;
                    borderCoords[cur] = new int2(room.Position.x + room.Dimensions.x, i);
                    ++cur;
                }

                for (int i = 0; i < borderCoords.Length; ++i)
                {
                    int2 coords = borderCoords[i];
                    int index = grid.ToIndex(coords);

                    if (grid.StaticEnvironmentLayer[index].LayerMask == EnvironmentTileLayerMask.Ground)
                    {
                        // S'il y a un sol à cet endroit, on regarde s'il est entouré de murs.
                        // Si oui, on lance l'aléa pour créer une porte ou non à cet endroit.

                        if (CanPlaceDoorAt(coords, grid) && rand.NextFloat(100f) < doorSpawnRate)
                        {
                            grid.DoorsLayer[index] = new DoorActor(doorTiles.Sample(ref rand));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Génère des rivières à travers le niveau
        /// </summary>
        /// <param name="tl">Contient les cases utilisés pour la génération</param>
        /// <param name="rgs">Paramètres de génération des rivières et lacs</param>
        /// <param name="grid">La grille</param>
        /// <param name="rand">Générateur d'aléatoire</param>
        private void GenerateRivers(TileLibrarySO tl, RiverGenerationSettingsSO rgs, Grid grid, ref Random rand)
        {
            int nbRiversToGenerate = rand.NextFloat(100f) < rgs.RiverSpawnRate ? rand.NextInt(rgs.NbRiversInterval.x, rgs.NbRiversInterval.y) : 0;

            for (int i = 0; i < nbRiversToGenerate; ++i)
            {
                RiverType type = rgs.RiverTypes.Sample(ref rand);
                IEnvironmentActor liquidTile = tl.RiverTiles[type];
                int width = rand.NextInt(rgs.RiverWidthInterval.x, rgs.RiverWidthInterval.y);
                System.Span<int2> directions = stackalloc int2[4] { new(1, 0), new(-1, 0), new(0, 1), new(0, -1) };
            }
        }

        /// <summary>
        /// Indique si une porte peut être placée à l'index donné
        /// </summary>
        /// <param name="coords">Coordonnées de la porte</param>
        /// <param name="grid">Grille de cases</param>
        /// <returns>true si une porte peut être placée à l'index donné</returns>
        private static bool CanPlaceDoorAt(int2 coords, Grid grid)
        {
            // Si la case est adjacente à deux murs, et que ces murs sont opposés l'un à l'autre,
            // la case est valide.

            IEnvironmentActor right = grid.StaticEnvironmentLayer[grid.ToIndex(coords + new int2(1, 0))];
            IEnvironmentActor left = grid.StaticEnvironmentLayer[grid.ToIndex(coords + new int2(-1, 0))];
            IEnvironmentActor up = grid.StaticEnvironmentLayer[grid.ToIndex(coords + new int2(0, 1))];
            IEnvironmentActor down = grid.StaticEnvironmentLayer[grid.ToIndex(coords + new int2(0, -1))];

            return (right.LayerMask == EnvironmentTileLayerMask.Wall && left.LayerMask == EnvironmentTileLayerMask.Wall) ||
                    (up.LayerMask == EnvironmentTileLayerMask.Wall && down.LayerMask == EnvironmentTileLayerMask.Wall);
        }

        #endregion
    }
}