using System.Collections.Generic;
using Assets.Scripts.Runtime.Models.Features;
using Assets.Scripts.Runtime.Models.Generation;
using Assets.Scripts.Runtime.Models.Tiles;
using Assets.Scripts.Runtime.Models.Tiles.TilePalette;
using Unity.Mathematics;
using Random = UnityEngine.Random;

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
        /// <param name="gridSize">Les dimensions de la grille</param>
        /// <returns>La grille des cases créées</returns>
        public static TileEntitySO[] GenerateEnvironmnent(RoomsAndCorridorsAlgorithmSettingsSO settings, TileLibrarySO tileLibrary, int2 gridSize)
        {
            int nbMaxRooms = Random.Range(settings.MinMaxNbRooms.x, settings.MinMaxNbRooms.y);
            int nbAttemps = 0;
            int nbRoomsCreated = 0;

            TileEntitySO[] environmentLayer = new TileEntitySO[gridSize.x * gridSize.y];
            _rooms.Clear();
            _corridors.Clear();

            // Remplit la carte de murs pour pouvoir en creuser les salles

            GenerationAlgUtils.FillMap(environmentLayer, gridSize, tileLibrary.WallTile);

            // Tant qu'on n'a pas atteint nbMaxRooms, on tente de générer des salles

            while (nbRoomsCreated < nbMaxRooms && nbAttemps < Constants.NB_MAX_ROOM_GENERATION_ATTEMPTS)
            {
                // On crée une nouvelle DungeonStructure avec une position et dimensions aléatoires

                int2 newDimensions = new(Random.Range(settings.MinMaxRoomSize.x, settings.MinMaxRoomSize.y), Random.Range(settings.MinMaxRoomSize.x, settings.MinMaxRoomSize.y));
                int2 newPos = new(Random.Range(1, gridSize.x - newDimensions.x), Random.Range(1, gridSize.y - newDimensions.y));
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

                GenerationAlgUtils.CreateRectangularRoom(gridSize, newPos, newDimensions, tileLibrary, environmentLayer);
                ++nbAttemps;
            }

            // Si on a pu créer plus d'une salle, on créer les corridors pour les connecter entre elles

            if (_rooms.Count > 1)
            {
                for (int i = 1; i < _rooms.Count; ++i)
                {
                    DungeonStructure curRoom = _rooms[i];
                    DungeonStructure previousRoom = _rooms[i - 1];

                    if (Random.Range(0, 2) == 0)
                    {
                        GenerationAlgUtils.CreateHorizontalTunnel(gridSize, previousRoom.Centroid.x, curRoom.Centroid.x, previousRoom.Centroid.y, tileLibrary, environmentLayer);
                        GenerationAlgUtils.CreateVerticalTunnel(gridSize, previousRoom.Centroid.y, curRoom.Centroid.y, curRoom.Centroid.x, tileLibrary, environmentLayer);
                    }
                    else
                    {
                        GenerationAlgUtils.CreateVerticalTunnel(gridSize, previousRoom.Centroid.y, curRoom.Centroid.y, curRoom.Centroid.x, tileLibrary, environmentLayer);
                        GenerationAlgUtils.CreateHorizontalTunnel(gridSize, previousRoom.Centroid.x, curRoom.Centroid.x, previousRoom.Centroid.y, tileLibrary, environmentLayer);
                    }
                }
            }

            return environmentLayer;
        }
    }
}