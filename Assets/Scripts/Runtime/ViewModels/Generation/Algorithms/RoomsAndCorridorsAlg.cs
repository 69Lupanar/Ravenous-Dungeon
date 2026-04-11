using System.Collections.Generic;
using Assets.Scripts.Runtime.Models.Generation;
using Assets.Scripts.Runtime.Models.Map;
using Assets.Scripts.Runtime.Models.Tiles.TilePalette;
using Assets.Scripts.Runtime.Models.ValueTypes;
using Unity.Mathematics;

namespace Assets.Scripts.Runtime.ViewModels.Generation.Algorithms
{
    /// <summary>
    /// Algorithme générant des salles connectées par des corridors
    /// </summary>
    public class RoomsAndCorridorsAlg
    {
        /// <summary>
        /// On recycle la liste pour ne pas consommer trop de mémoire
        /// </summary>
        private static readonly List<DungeonStructure> _rooms = new();

        /// <summary>
        /// On recycle la liste pour ne pas consommer trop de mémoire
        /// </summary>
        private static readonly List<DungeonStructure> _corridors = new();

        /// <summary>
        /// Génère des salles connectées entre elles par des corridors
        /// </summary>
        /// <param name="settings">Paramètres de génération</param>
        /// <param name="tileLibrary">Contient les cases utilisés pour la génération</param>
        /// <param name="grid">La grille</param>
        /// <param name="rand">Générateur d'aléatoire</param>
        /// <returns>La grille des cases créées</returns>
        public static void GenerateEnvironmnent(RoomsAndCorridorsAlgorithmSettingsSO settings, TileLibrarySO tileLibrary, Grid grid, ref Unity.Mathematics.Random rand)
        {
            int nbMaxRooms = rand.NextInt(settings.NbRoomsInterval.x, settings.NbRoomsInterval.y);
            int nbAttemps = 0;
            int nbRoomsCreated = 0;

            _rooms.Clear();
            _corridors.Clear();

            // Remplit la carte de murs pour pouvoir en creuser les salles

            GenerationAlgUtils.FillMap(grid.EnvironmentLayer, grid.GridSize, tileLibrary.WallTiles);

            // Tant qu'on n'a pas atteint nbMaxRooms, on tente de générer des salles

            while (nbRoomsCreated < nbMaxRooms && nbAttemps < Constants.NB_MAX_ROOM_GENERATION_ATTEMPTS)
            {
                // On crée une nouvelle DungeonStructure avec une position et dimensions aléatoires

                int2 newDimensions = new(rand.NextInt(settings.RoomSizeInterval.x, settings.RoomSizeInterval.y), rand.NextInt(settings.RoomSizeInterval.x, settings.RoomSizeInterval.y));
                int2 newPos = new(rand.NextInt(1, grid.GridSize.x - newDimensions.x), rand.NextInt(1, grid.GridSize.y - newDimensions.y));
                DungeonStructure newRoom = new(newPos, newDimensions);

                // Parmi les salles déjà créées, s'il y en a déjà une qui se superpose, on arrête la tentative

                bool overlaps = false;

                foreach (DungeonStructure room in _rooms)
                {
                    if (room.Overlaps(newRoom))
                    {
                        overlaps = true;
                        break;
                    }
                }

                if (overlaps)
                {
                    ++nbAttemps;
                    continue;
                }

                // Sinon, on génère la salle

                _rooms.Add(newRoom);

                GenerationAlgUtils.CreateRectangularRoom(grid.GridSize, newPos, newDimensions, tileLibrary, grid.EnvironmentLayer);
                ++nbAttemps;
            }

            // Si on a pu créer plus d'une salle, on créer les corridors pour les connecter entre elles

            if (_rooms.Count > 1)
            {
                for (int i = 1; i < _rooms.Count; ++i)
                {
                    DungeonStructure curRoom = _rooms[i];
                    DungeonStructure previousRoom = _rooms[i - 1];

                    if (rand.NextBool())
                    {
                        GenerationAlgUtils.CreateHorizontalTunnel(grid.GridSize, previousRoom.Centroid.x, curRoom.Centroid.x, previousRoom.Centroid.y, tileLibrary, grid.EnvironmentLayer);
                        GenerationAlgUtils.CreateVerticalTunnel(grid.GridSize, previousRoom.Centroid.y, curRoom.Centroid.y, curRoom.Centroid.x, tileLibrary, grid.EnvironmentLayer);
                    }
                    else
                    {
                        GenerationAlgUtils.CreateVerticalTunnel(grid.GridSize, previousRoom.Centroid.y, curRoom.Centroid.y, curRoom.Centroid.x, tileLibrary, grid.EnvironmentLayer);
                        GenerationAlgUtils.CreateHorizontalTunnel(grid.GridSize, previousRoom.Centroid.x, curRoom.Centroid.x, previousRoom.Centroid.y, tileLibrary, grid.EnvironmentLayer);
                    }
                }
            }

            grid.Rooms = _rooms.ToArray();
            grid.Corridors = _corridors.ToArray();
        }
    }
}