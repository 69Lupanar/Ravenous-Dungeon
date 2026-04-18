using Assets.Scripts.Runtime.Models.Actors;
using Assets.Scripts.Runtime.Models.Map;
using Assets.Scripts.Runtime.Models.Tiles;
using Assets.Scripts.Runtime.Models.Tiles.TilePalette;
using Assets.Scripts.Runtime.Models.ValueTypes;
using Assets.Scripts.Runtime.ViewModels.Extensions;
using Unity.Collections;
using Unity.Mathematics;

namespace Assets.Scripts.Runtime.ViewModels.Generation.MapGeneration
{
    /// <summary>
    /// Contient les méthodes des algorithmes de génération
    /// </summary>
    internal static class MapGenerationUtils
    {
        #region Méthodes publiques

        /// <summary>
        /// Remplit la carte entière d'un seul type de case
        /// </summary>
        internal static void FillMapWithWalls(StaticEnvironmentActor[] environmentLayer, int2 gridSize, ItemSelectionChance<StaticEnvironmentTileSO>[] tiles)
        {
            for (int y = 0; y < gridSize.y; ++y)
            {
                for (int x = 0; x < gridSize.x; ++x)
                {
                    int index = x + y * gridSize.x;
                    environmentLayer[index] = new StaticEnvironmentActor(tiles.Sample());

                    // Si c'est les limites de la carte, on marque les murs comme indestructibles

                    if (y == 0 || y == gridSize.x - 1 || x == 0 && x == gridSize.x - 1)
                    {
                        ref StaticEnvironmentActor actor = ref environmentLayer[index];
                        actor.Attributes |= TileAttributes.Indestructible;
                    }
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
        internal static void CreateRectangularRoom(int2 gridSize, int2 position, int2 dimensions, TileLibrarySO tileLibrary, StaticEnvironmentActor[] environmentLayer)
        {
            for (int y = position.y; y < position.y + dimensions.y; ++y)
            {
                for (int x = position.x; x < position.x + dimensions.x; ++x)
                {
                    environmentLayer[x + y * gridSize.x] = new StaticEnvironmentActor(tileLibrary.GroundTile);
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
        internal static void CreateHorizontalTunnel(int2 gridSize, int xStart, int xEnd, int yPosition, TileLibrarySO tileLibrary, StaticEnvironmentActor[] environmentLayer)
        {
            for (int x = math.min(xStart, xEnd); x <= math.max(xStart, xEnd); ++x)
            {
                environmentLayer[x + yPosition * gridSize.x] = new StaticEnvironmentActor(tileLibrary.GroundTile);
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
        internal static void CreateVerticalTunnel(int2 gridSize, int yStart, int yEnd, int xPosition, TileLibrarySO tileLibrary, StaticEnvironmentActor[] environmentLayer)
        {
            for (int y = math.min(yStart, yEnd); y <= math.max(yStart, yEnd); ++y)
            {
                environmentLayer[xPosition + y * gridSize.x] = new StaticEnvironmentActor(tileLibrary.GroundTile);
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
        internal static void CreateRiver(TileLibrarySO tl, Grid grid, LiquidType type, NativeArray<int2> path, ref Random rand)
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