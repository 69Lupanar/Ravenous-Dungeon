using Assets.Scripts.Runtime.Models.Generation;
using Assets.Scripts.Runtime.Models.Map;
using Assets.Scripts.Runtime.Models.Tiles;
using Assets.Scripts.Runtime.Models.Tiles.TilePalette;
using Assets.Scripts.Runtime.Models.ValueTypes;
using Assets.Scripts.Runtime.ViewModels.Extensions;
using Unity.Mathematics;

namespace Assets.Scripts.Runtime.ViewModels.Generation.Algorithms
{
    /// <summary>
    /// Contient les méthodes des algorithmes de génération
    /// </summary>
    internal static class GenerationAlgUtils
    {
        #region Méthodes publiques

        #region General

        /// <summary>
        /// Génère une nouvelle carte
        /// </summary>
        /// <param name="tl">Contient les cases utilisés pour la génération</param>
        /// <param name="alg">Algorithme de génération sélectionné</param>
        /// <param name="grid">La grille</param>
        /// <param name="rand">Générateur d'aléatoire</param>
        internal static void GenerateEnvironmnent(TileLibrarySO tl, GenerationAlgorithmSettingsSO alg, Grid grid, ref Random rand)
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
        internal static void GenerateFeatures(TileLibrarySO tl, GenerationAlgorithmSettingsSO alg, Grid grid, ref Random rand)
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
        /// Génère des rivières à travers le niveau
        /// </summary>
        /// <param name="tl">Contient les cases utilisés pour la génération</param>
        /// <param name="riverTypes">Les types de rivière pouvant être générées</param>
        /// <param name="riverWidthInterval">L'intervalle possible de largeur des rivières générées</param>
        /// <param name="nbRiversToGenerate">Nb de rivières à générer</param>
        /// <param name="grid">La grille</param>
        /// <param name="rand">Générateur d'aléatoire</param>
        internal static void GenerateRivers(TileLibrarySO tl, ItemSelectionChance<RiverType>[] riverTypes, int2 riverWidthInterval, int nbRiversToGenerate, Grid grid, ref Random rand)
        {
            for (int i = 0; i < nbRiversToGenerate; ++i)
            {
                RiverType type = riverTypes.Sample(ref rand);
                EnvironmentTileSO liquidTile = tl.RiverTiles[type];
                int width = rand.NextInt(riverWidthInterval.x, riverWidthInterval.y);
            }
        }

        #endregion

        #region Dungeon Structures

        /// <summary>
        /// Remplit la carte entière d'un seul type de case
        /// </summary>
        internal static void FillMap(EnvironmentTileSO[] environmentLayer, int2 gridSize, EnvironmentTileSO tile)
        {
            for (int y = 0; y < gridSize.y; ++y)
            {
                for (int x = 0; x < gridSize.x; ++x)
                {
                    environmentLayer[x + y * gridSize.x] = tile;
                }
            }
        }

        /// <summary>
        /// Remplit la carte entière d'un seul type de case
        /// </summary>
        internal static void FillMap(EnvironmentTileSO[] environmentLayer, int2 gridSize, ItemSelectionChance<EnvironmentTileSO>[] tiles)
        {
            for (int y = 0; y < gridSize.y; ++y)
            {
                for (int x = 0; x < gridSize.x; ++x)
                {
                    environmentLayer[x + y * gridSize.x] = tiles.Sample();
                }
            }
        }

        /// <summary>
        /// Génère une structure rectangulaire
        /// </summary>
        /// <param name="gridSize">Dimensions de la grille</param>
        /// <param name="position">La position de la structure</param>
        /// <param name="dimensions">Les dimensions de la structure</param>
        /// <param name="tileLibrary">Contient les cases utilisés pour la génération</param>
        /// <param name="environmentLayer">Couche de la tilemap représentant l'environnement</param>
        internal static void CreateRectangularRoom(int2 gridSize, int2 position, int2 dimensions, TileLibrarySO tileLibrary, EnvironmentTileSO[] environmentLayer)
        {
            for (int y = position.y; y < position.y + dimensions.y; ++y)
            {
                for (int x = position.x; x < position.x + dimensions.x; ++x)
                {
                    environmentLayer[x + y * gridSize.x] = tileLibrary.GroundTile;
                }
            }
        }


        /// <summary>
        /// Carve a tunnel out of the map parallel to the x-axis
        /// </summary>
        /// <param name="gridSize"></param>
        /// <param name="xStart"></param>
        /// <param name="xEnd"></param>
        /// <param name="yPosition"></param>
        /// <param name="tileLibrary"></param>
        /// <param name="environmentLayer"></param>
        internal static void CreateHorizontalTunnel(int2 gridSize, int xStart, int xEnd, int yPosition, TileLibrarySO tileLibrary, EnvironmentTileSO[] environmentLayer)
        {
            for (int x = math.min(xStart, xEnd); x <= math.max(xStart, xEnd); ++x)
            {
                environmentLayer[x + yPosition * gridSize.x] = tileLibrary.GroundTile;
            }
        }

        /// <summary>
        /// Carve a tunnel out of the map parallel to the y-axis
        /// </summary>
        /// <param name="gridSize"></param>
        /// <param name="yStart"></param>
        /// <param name="yEnd"></param>
        /// <param name="xPosition"></param>
        /// <param name="tileLibrary"></param>
        /// <param name="environmentLayer"></param>
        internal static void CreateVerticalTunnel(int2 gridSize, int yStart, int yEnd, int xPosition, TileLibrarySO tileLibrary, EnvironmentTileSO[] environmentLayer)
        {
            for (int y = math.min(yStart, yEnd); y <= math.max(yStart, yEnd); ++y)
            {
                environmentLayer[xPosition + y * gridSize.x] = tileLibrary.GroundTile;
            }
        }

        /// <summary>
        /// Génère des portes aux entrées des salles
        /// </summary>
        /// <param name="doorTiles">Liste des cases possibles pour représenter les portes</param>
        /// <param name="doorSpawnRate">%age de chance de créer une porte à une position donnée</param>
        /// <param name="grid">La grille</param>
        /// <param name="rand">Générateur d'aléatoire</param>
        private static void GenerateDoors(ItemSelectionChance<FeatureTileSO>[] doorTiles, int doorSpawnRate, Grid grid, ref Random rand)
        {
            foreach (DungeonStructure room in grid.Rooms)
            {
                System.Span<int2> borderCoords = stackalloc int2[(room.Position.x + room.Dimensions.x) * 2 + (room.Position.y + room.Dimensions.y) * 2];
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

                    if (grid.EnvironmentLayer[index].LayerMask == EnvironmentTileLayerMask.Ground)
                    {
                        // S'il y a un sol à cet endroit, on regarde s'il est entouré de murs.
                        // Si oui, on lance l'aléa pour créer une porte ou non à cet endroit.

                        if (CanPlaceDoorAt(coords, grid) && rand.NextFloat(100f) < doorSpawnRate)
                        {
                            grid.FeaturesLayer[index] = doorTiles.Sample(ref rand);
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

            EnvironmentTileSO right = grid.EnvironmentLayer[grid.ToIndex(coords + new int2(1, 0))];
            EnvironmentTileSO left = grid.EnvironmentLayer[grid.ToIndex(coords + new int2(-1, 0))];
            EnvironmentTileSO up = grid.EnvironmentLayer[grid.ToIndex(coords + new int2(0, 1))];
            EnvironmentTileSO down = grid.EnvironmentLayer[grid.ToIndex(coords + new int2(0, -1))];

            return (right.LayerMask == EnvironmentTileLayerMask.Wall && left.LayerMask == EnvironmentTileLayerMask.Wall) ||
                    (up.LayerMask == EnvironmentTileLayerMask.Wall && down.LayerMask == EnvironmentTileLayerMask.Wall);
        }

        #endregion

        #endregion
    }
}