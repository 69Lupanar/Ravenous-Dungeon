using System;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.MapGeneration
{
    /// <summary>
    /// G�n�re une nouvelle carte
    /// </summary>
    public class MapGenerator : MonoBehaviour
    {
        #region Evénements

        /// <summary>
        /// Appel� quand la g�n�ration est termin�e
        /// </summary>
        public EventHandler<GenerationEndedEventArgs> OnGenerationEnded;

        #endregion

        #region Variables Unity

        /// <summary>
        /// Liste de param�tres de g�n�ration possibles
        /// </summary>
        [SerializeField]
        private MapGenerationSettingsSO[] _generationSettings;

        /// <summary>
        /// Le PlayerController
        /// </summary>
        [SerializeField]
        private PlayerController _playerController;

        /// <summary>
        /// Graine de g�n�ration
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
        /// G�n�rateur d'al�atoire
        /// </summary>
        private Unity.Mathematics.Random _rand;

        #endregion

        #region M�thodes Unity

#if UNITY_EDITOR

        /// <summary>
        /// Appel�e quand modif dans l'inspecteur
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
        /// m�j � chaque frame
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

        #region M�thodes publiques

        /// <summary>
        /// G�n�re une nouvelle carte
        /// </summary>
        [ContextMenu("Generate")]
        public void Generate()
        {
            MapGenerationSettingsSO gs = _generationSettings[_rand.NextInt(_generationSettings.Length)];
            //GenerateRandomMap(gs);
        }

        ///// <summary>
        ///// G�n�re une nouvelle carte
        ///// </summary>
        ///// <param name="gs">Param�tres de g�n�ration</param>
        //public void GenerateRandomMap(MapGenerationSettingsSO gs)
        //{
        //    int2 gridSize = new(_rand.NextInt(gs.GridSizeInterval.x, gs.GridSizeInterval.y), _rand.NextInt(gs.GridSizeInterval.x, gs.GridSizeInterval.y));

        //    _grid = new Grid(gridSize);
        //    MapGenerationAlgorithmSettingsSO selectedAlg = gs.Algorithms.Sample(ref _rand);
        //    LiquidMapGenerationSettingsSO selectedRiverSettings = gs.RiverGenerationSettings.Sample(ref _rand);

        //    // G�n�re l'environnement

        //    GenerateEnvironmnent(gs.TileLibrary, selectedAlg, _grid, ref _rand);

        //    // G�n�re les liquides

        //    if (selectedRiverSettings != null)
        //    {
        //        GenerateRivers(gs.TileLibrary, selectedRiverSettings, _grid, ref _rand);
        //    }

        //    // G�n�re les �l�ments interactifs

        //    GenerateInteractables(gs.TileLibrary, selectedAlg, _grid, ref _rand);

        //    OnGenerationEnded?.Invoke(this, new GenerationEndedEventArgs(_grid, gs.SpriteLibrary));

        //    // Place le joueur sur la carte

        //    _playerController.SetGrid(_grid);
        //    _playerController.SpawnPlayer(_grid, ref _rand);
        //}

        //#endregion

        //#region M�thodes priv�es

        ///// <summary>
        ///// G�n�re une nouvelle carte
        ///// </summary>
        ///// <param name="tl">Contient les cases utilis�s pour la g�n�ration</param>
        ///// <param name="alg">Algorithme de g�n�ration s�lectionn�</param>
        ///// <param name="grid">La grille</param>
        ///// <param name="rand">G�n�rateur d'al�atoire</param>
        //private void GenerateEnvironmnent(TileLibrarySO tl, MapGenerationAlgorithmSettingsSO alg, Grid grid, ref Random rand)
        //{
        //    switch (alg)
        //    {
        //        case OneRoomAlgorithmSettingsSO:
        //            OneRoomMapGeneration.GenerateEnvironmnent(tl, grid);
        //            break;
        //        case RoomsAndCorridorsAlgorithmSettingsSO settings:
        //            RoomsAndCorridorsMapGeneration.GenerateEnvironmnent(settings, tl, grid, ref rand);
        //            break;
        //    }
        //}

        ///// <summary>
        ///// G�n�re une nouvelle carte
        ///// </summary>
        ///// <param name="tl">Contient les cases utilis�s pour la g�n�ration</param>
        ///// <param name="alg">Algorithme de g�n�ration s�lectionn�</param>
        ///// <param name="grid">La grille</param>
        ///// <param name="rand">G�n�rateur d'al�atoire</param>
        //private void GenerateInteractables(TileLibrarySO tl, MapGenerationAlgorithmSettingsSO alg, Grid grid, ref Random rand)
        //{
        //    switch (alg)
        //    {
        //        case OneRoomAlgorithmSettingsSO:

        //            break;

        //        case RoomsAndCorridorsAlgorithmSettingsSO settings:

        //            GenerateDoors(tl.DoorTiles, settings.DoorSpawnRate, grid, ref rand);
        //            break;
        //    }
        //}

        ///// <summary>
        ///// G�n�re des portes aux entr�es des salles
        ///// </summary>
        ///// <param name="doorTiles">Liste des cases possibles pour repr�senter les portes</param>
        ///// <param name="doorSpawnRate">%age de chance de cr�er une porte � une position donn�e</param>
        ///// <param name="grid">La grille</param>
        ///// <param name="rand">G�n�rateur d'al�atoire</param>
        //private static void GenerateDoors(ItemSelectionChance<DoorTileSO>[] doorTiles, int doorSpawnRate, Grid grid, ref Random rand)
        //{
        //    foreach (DungeonStructure room in grid.Rooms)
        //    {
        //        Span<int2> borderCoords = stackalloc int2[(room.Position.x + room.Dimensions.x) * 2 + (room.Position.y + room.Dimensions.y) * 2];
        //        int cur = 0;

        //        // Mur du bas et du haut

        //        for (int i = room.Position.x; i < room.Position.x + room.Dimensions.x; ++i)
        //        {
        //            borderCoords[cur] = new int2(i, room.Position.y - 1);
        //            ++cur;
        //            borderCoords[cur] = new int2(i, room.Position.y + room.Dimensions.y);
        //            ++cur;
        //        }

        //        // Mur de gauche et de droite

        //        for (int i = room.Position.y; i < room.Position.y + room.Dimensions.y; ++i)
        //        {
        //            borderCoords[cur] = new int2(room.Position.x - 1, i);
        //            ++cur;
        //            borderCoords[cur] = new int2(room.Position.x + room.Dimensions.x, i);
        //            ++cur;
        //        }

        //        for (int i = 0; i < borderCoords.Length; ++i)
        //        {
        //            int2 coords = borderCoords[i];
        //            int index = grid.ToIndex(coords);

        //            if (grid.StaticEnvironmentLayer[index].LayerMask == EnvironmentTileLayerMask.Ground)
        //            {
        //                // S'il y a un sol � cet endroit, on regarde s'il est entour� de murs.
        //                // Si oui, on lance l'al�a pour cr�er une porte ou non � cet endroit.

        //                if (CanPlaceDoorAt(coords, grid) && rand.NextFloat(100f) < doorSpawnRate)
        //                {
        //                    grid.DoorsLayer[index] = new DoorActor(doorTiles.Sample(ref rand));
        //                }
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// Indique si une porte peut �tre plac�e � l'index donn�
        ///// </summary>
        ///// <param name="coords">Coordonn�es de la porte</param>
        ///// <param name="grid">Grille de cases</param>
        ///// <returns>true si une porte peut �tre plac�e � l'index donn�</returns>
        //private static bool CanPlaceDoorAt(int2 coords, Grid grid)
        //{
        //    // Si la case est adjacente � deux murs, et que ces murs sont oppos�s l'un � l'autre,
        //    // la case est valide.

        //    IEnvironmentActor right = grid.StaticEnvironmentLayer[grid.ToIndex(coords + new int2(1, 0))];
        //    IEnvironmentActor left = grid.StaticEnvironmentLayer[grid.ToIndex(coords + new int2(-1, 0))];
        //    IEnvironmentActor up = grid.StaticEnvironmentLayer[grid.ToIndex(coords + new int2(0, 1))];
        //    IEnvironmentActor down = grid.StaticEnvironmentLayer[grid.ToIndex(coords + new int2(0, -1))];

        //    return (right.LayerMask == EnvironmentTileLayerMask.Wall && left.LayerMask == EnvironmentTileLayerMask.Wall) ||
        //            (up.LayerMask == EnvironmentTileLayerMask.Wall && down.LayerMask == EnvironmentTileLayerMask.Wall);
        //}

        ///// <summary>
        ///// G�n�re des rivi�res � travers le niveau
        ///// </summary>
        ///// <param name="tl">Contient les cases utilis�s pour la g�n�ration</param>
        ///// <param name="lgs">Param�tres de g�n�ration des rivi�res et lacs</param>
        ///// <param name="grid">La grille</param>
        ///// <param name="rand">G�n�rateur d'al�atoire</param>
        //private void GenerateRivers(TileLibrarySO tl, LiquidMapGenerationSettingsSO lgs, Grid grid, ref Random rand)
        //{
        //    int nbRiversToGenerate = rand.NextFloat(100f) < lgs.RiverSpawnRate ? rand.NextInt(lgs.NbRiversInterval.x, lgs.NbRiversInterval.y) : 0;
        //    int nbLakesToGenerate = rand.NextFloat(100f) < lgs.LakeSpawnRate ? rand.NextInt(lgs.NbLakesInterval.x, lgs.NbLakesInterval.y) : 0;

        //    for (int i = 0; i < nbRiversToGenerate; ++i)
        //    {
        //        int nbForks = rand.NextFloat(100f) < lgs.RiverForkSpawnRate ? rand.NextInt(lgs.NbRiversForksInterval.x, lgs.NbRiversForksInterval.y) : 0;
        //        int width = rand.NextInt(lgs.RiverWidthInterval.x, lgs.RiverWidthInterval.y);

        //        // S�lectionne un type de liquide au hasard.
        //        // Chaque case dans le tableau correspond � diff�rents niveaux de force du liquide.

        //        LiquidType type = lgs.LiquidTypes.Sample(ref rand);

        //        // On s�lectionne 2 bords de la carte au hasard comme points de d�part/fin

        //        int randomStartEdge = rand.NextInt(0, 4);
        //        int randomEndEdge = rand.NextInt(0, 4);

        //        if (!lgs.AllowForkToReturnToStartingEdge)
        //        {
        //            // On s'assure que le mur s�lectionn� n'est pas celui d'origine

        //            while (randomEndEdge == randomStartEdge)
        //            {
        //                randomEndEdge = rand.NextInt(0, 4);
        //            }
        //        }

        //        grid.GetPointOnMapEdge(randomStartEdge, width, 1, ref rand, out int2 start);
        //        grid.GetPointOnMapEdge(randomEndEdge, width, 1, ref rand, out int2 end);

        //        // On g�n�re le chemin

        //        MapGenerationUtils.CreateNoise(grid.GridSize, lgs.NoiseFactor, lgs.NoiseScale, out NativeArray<float> noise);
        //        AStarPathfinding.GetPath(start, end, grid.GridSize, noise, out NativeArray<int2> path);
        //        MapGenerationUtils.CreateRiver(tl, grid, type, width, path, ref rand);

        //        // On cr�e des branches si besoin

        //        for (int j = 0; j < nbForks; ++j)
        //        {
        //            width = rand.NextInt(lgs.RiverWidthInterval.x, lgs.RiverWidthInterval.y);
        //            int2 randomPointOnRiver = path[rand.NextInt(path.Length)];

        //            if (!lgs.AllowForkToReturnToStartingEdge)
        //            {
        //                // On s'assure que le mur s�lectionn� n'est pas celui d'origine

        //                do
        //                {
        //                    randomEndEdge = rand.NextInt(0, 4);
        //                }
        //                while (randomEndEdge == randomStartEdge);
        //            }

        //            grid.GetPointOnMapEdge(randomEndEdge, width, 1, ref rand, out end);
        //            AStarPathfinding.GetPath(randomPointOnRiver, end, grid.GridSize, noise, out path);
        //            MapGenerationUtils.CreateRiver(tl, grid, type, width, path, ref rand);
        //        }
        //    }
        //}

        #endregion
    }
}