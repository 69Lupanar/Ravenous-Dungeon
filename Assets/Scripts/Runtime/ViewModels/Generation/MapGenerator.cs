using System;
using Assets.Scripts.Runtime.Models.Actors;
using Assets.Scripts.Runtime.Models.Generation;
using Assets.Scripts.Runtime.Models.Generation.Algorithms;
using Assets.Scripts.Runtime.Models.Tiles;
using Assets.Scripts.Runtime.Models.Tiles.TilePalette;
using Assets.Scripts.Runtime.Models.ValueTypes;
using Assets.Scripts.Runtime.ViewModels.Extensions;
using Assets.Scripts.Runtime.ViewModels.Generation.MapGeneration;
using Assets.Scripts.Runtime.ViewModels.Generation.Pathfinding;
using Assets.Scripts.Runtime.ViewModels.Player;
using Unity.Collections;
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
            LiquidGenerationSettingsSO selectedRiverSettings = gs.RiverGenerationSettings.Sample(ref _rand);

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
                    OneRoomMapGeneration.GenerateEnvironmnent(tl, grid);
                    break;
                case RoomsAndCorridorsAlgorithmSettingsSO settings:
                    RoomsAndCorridorsMapGeneration.GenerateEnvironmnent(settings, tl, grid, ref rand);
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

        /// <summary>
        /// Génère des rivières à travers le niveau
        /// </summary>
        /// <param name="tl">Contient les cases utilisés pour la génération</param>
        /// <param name="lgs">Paramètres de génération des rivières et lacs</param>
        /// <param name="grid">La grille</param>
        /// <param name="rand">Générateur d'aléatoire</param>
        private void GenerateRivers(TileLibrarySO tl, LiquidGenerationSettingsSO lgs, Grid grid, ref Random rand)
        {
            int nbRiversToGenerate = rand.NextFloat(100f) < lgs.RiverSpawnRate ? rand.NextInt(lgs.NbRiversInterval.x, lgs.NbRiversInterval.y) : 0;
            int nbLakesToGenerate = rand.NextFloat(100f) < lgs.LakeSpawnRate ? rand.NextInt(lgs.NbLakesInterval.x, lgs.NbLakesInterval.y) : 0;

            for (int i = 0; i < nbRiversToGenerate; ++i)
            {
                int nbForks = rand.NextFloat(100f) < lgs.RiverForkSpawnRate ? rand.NextInt(lgs.NbRiversForksInterval.x, lgs.NbRiversForksInterval.y) : 0;
                int width = rand.NextInt(lgs.RiverWidthInterval.x, lgs.RiverWidthInterval.y);

                // Sélectionne un type de liquide au hasard.
                // Chaque case dans le tableau correspond à différents niveaux de force du liquide.

                LiquidType type = lgs.LiquidTypes.Sample(ref rand);

                // On sélectionne 2 bords de la carte au hasard comme points de départ/fin

                int randomStartEdge = rand.NextInt(0, 4);
                int randomEndEdge = rand.NextInt(0, 4);

                grid.GetPointOnMapEdge(randomStartEdge, width, 1, ref rand, out int2 start);
                grid.GetPointOnMapEdge(randomEndEdge, width, 1, ref rand, out int2 end);

                // On génère le chemin

                AStarPathfinding.GetPath(start, end, grid.GridSize, ref rand, out NativeArray<int2> path);
                SetRiverTiles(tl, grid, type, path, ref rand);

                // On crée des branches si besoin

                for (int j = 0; j < nbForks; ++j)
                {
                    int2 randomPointOnRiver = path[rand.NextInt(path.Length)];

                    if (!lgs.AllowForkToReturnToStartingEdge)
                    {
                        // On s'assure que le mur sélectionné n'est pas celui d'origine

                        do
                        {
                            randomEndEdge = rand.NextInt(0, 4);
                        }
                        while (randomEndEdge == randomStartEdge);
                    }

                    grid.GetPointOnMapEdge(randomEndEdge, width, 1, ref rand, out end);
                    AStarPathfinding.GetPath(randomPointOnRiver, end, grid.GridSize, ref rand, out path);
                    SetRiverTiles(tl, grid, type, path, ref rand);
                }
            }
        }

        /// <summary>
        /// Retire toutes les cases destructibles
        /// pour y placer des zones liquides
        /// </summary>
        /// <param name="tl">Contient les cases utilisés pour la génération</param>
        /// <param name="grid">La grille</param>
        /// <param name="type">Le type de liquide utilisé</param>
        /// <param name="path">Chemin de la rivière</param>
        /// <param name="rand">Générateur d'aléatoire</param>
        private static void SetRiverTiles(TileLibrarySO tl, Grid grid, LiquidType type, NativeArray<int2> path, ref Random rand)
        {
            for (int j = 0; j < path.Length; ++j)
            {
                int index = grid.ToIndex(path[j]);
                LiquidTileSO liquidTile = tl.RiverTiles[type].Sample(ref rand);

                if (!grid.StaticEnvironmentLayer[index].Attributes.HasFlag(TileAttributes.Indestructible) &&
                    !grid.InteractablesLayer[index].Attributes.HasFlag(TileAttributes.Indestructible))
                {
                    grid.StaticEnvironmentLayer[index] = default;
                    grid.DoorsLayer[index] = default;
                    grid.InteractablesLayer[index] = default;
                    grid.LiquidsLayer[index] = new LiquidActor(liquidTile);
                }
            }
        }

        #endregion
    }
}