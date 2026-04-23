using System.Runtime.CompilerServices;
using Assets.Scripts.Runtime.ViewModels.Generation.LiquidGeneration;
using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Mathematics;

namespace Assets.Scripts.Runtime.ViewModels.Generation.Pathfinding
{
    /// <summary>
    /// Génère les rivières et lacs de la carte
    /// </summary>
    [BurstCompile]
    public static class AStarPathfinding
    {
        #region Constants

        /// <summary>
        /// Coût de navigation d'une case
        /// </summary>
        private const float MOVEMENT_COST = 10f;

        /// <summary>
        /// Force du noise sur la direction du pathfinding
        /// </summary>
        private const float NOISE_STRENGTH = 50f;

        #endregion

        #region Méthodes internes

        /// <summary>
        /// Génère une rivière
        /// </summary>
        /// <param name="startPos">Début de la rivière</param>
        /// <param name="endPos">Fin de la rivière</param>
        /// <param name="gridSize">Dimensions de la grille</param>
        /// <param name="rand">Générateur d'aléatoire</param>
        ///  <param name="result">Liste des coordonnées où placer des cases de liquide</param>
        [BurstCompile, SkipLocalsInit]
        internal static void GetPath(in int2 startPos, in int2 endPos, in int2 gridSize, out NativeArray<int2> result)
        {
            result = default;
            NativeArray<AStarNode> grid = new(gridSize.x * gridSize.y, Allocator.Temp);

            for (int y = 0; y < gridSize.y; ++y)
            {
                for (int x = 0; x < gridSize.x; ++x)
                {
                    int2 pos = new(x, y);
                    grid[ToIndex(pos, gridSize.x)] = new AStarNode(pos);
                }
            }

            NativeList<AStarNode> openSet = new(1, Allocator.Temp);
            NativeList<AStarNode> closedSet = new(1, Allocator.Temp);

            openSet.Add(grid[ToIndex(startPos, gridSize.x)]);

            while (openSet.Length > 0)
            {
                AStarNode currentNode = openSet[0];
                int index = 0;

                for (int i = 1; i < openSet.Length; ++i)
                {
                    if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost)
                    {
                        currentNode = openSet[i];
                        index = i;
                    }
                }

                openSet.RemoveAt(index);
                closedSet.Add(currentNode);

                if (math.all(currentNode.Position == endPos))
                {
                    RetracePath(grid, gridSize, startPos, endPos, out result);
                    return;
                }

                GetNeighbours(currentNode.Position, grid, gridSize, out NativeArray<AStarNode> neighbours);

                for (int i = 0; i < neighbours.Length; ++i)
                {
                    AStarNode neighbour = neighbours[i];

                    if (closedSet.Contains(neighbour))
                        continue;

                    float newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode.Position, neighbour.Position);
                    bool openSetContainsNeighbour = openSet.Contains(neighbour);

                    if (newMovementCostToNeighbour < neighbour.GCost || !openSetContainsNeighbour)
                    {
                        neighbour.GCost = newMovementCostToNeighbour;
                        neighbour.HCost = GetDistance(neighbour.Position, endPos);
                        neighbour.ParentPos = currentNode.Position;
                        grid[ToIndex(neighbour.Position, gridSize.x)] = neighbour;

                        if (!openSetContainsNeighbour)
                            openSet.Add(neighbour);
                    }
                }
            }
        }

        /// <summary>
        /// Génère une rivière
        /// </summary>
        /// <param name="startPos">Début de la rivière</param>
        /// <param name="endPos">Fin de la rivière</param>
        /// <param name="gridSize">Dimensions de la grille</param>
        /// <param name="noiseGrid">Permet d'altérer aléatoirement le poids de chaque cellule</param>
        ///  <param name="result">Liste des coordonnées où placer des cases de liquide</param>
        [BurstCompile, SkipLocalsInit]
        internal static void GetPath(in int2 startPos, in int2 endPos, in int2 gridSize, in NativeArray<float> noiseGrid, out NativeArray<int2> result)
        {
            NativeArray<AStarNode> grid = new(gridSize.x * gridSize.y, Allocator.Temp);

            for (int y = 0; y < gridSize.y; ++y)
            {
                for (int x = 0; x < gridSize.x; ++x)
                {
                    int2 pos = new(x, y);
                    grid[ToIndex(pos, gridSize.x)] = new AStarNode(pos);
                }
            }

            NativeList<AStarNode> openSet = new(1, Allocator.Temp);
            NativeList<AStarNode> closedSet = new(1, Allocator.Temp);

            openSet.Add(grid[ToIndex(startPos, gridSize.x)]);

            while (openSet.Length > 0)
            {
                AStarNode currentNode = openSet[0];
                int index = 0;

                for (int i = 1; i < openSet.Length; ++i)
                {
                    if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost)
                    {
                        currentNode = openSet[i];
                        index = i;
                    }
                }

                openSet.RemoveAt(index);
                closedSet.Add(currentNode);

                if (math.all(currentNode.Position == endPos))
                {
                    RetracePath(grid, gridSize, startPos, endPos, out result);
                    return;
                }

                GetNeighbours(currentNode.Position, grid, gridSize, out NativeArray<AStarNode> neighbours);

                for (int i = 0; i < neighbours.Length; ++i)
                {
                    AStarNode neighbour = neighbours[i];

                    if (closedSet.Contains(neighbour))
                        continue;

                    int noiseWeight = (int)(noiseGrid[ToIndex(neighbour.Position, gridSize.x)] * NOISE_STRENGTH);
                    float newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode.Position, neighbour.Position) + noiseWeight;
                    bool openSetContainsNeighbour = openSet.Contains(neighbour);

                    if (newMovementCostToNeighbour < neighbour.GCost || !openSetContainsNeighbour)
                    {
                        neighbour.GCost = newMovementCostToNeighbour;
                        neighbour.HCost = GetDistance(neighbour.Position, endPos) - noiseWeight;
                        neighbour.ParentPos = currentNode.Position;
                        grid[ToIndex(neighbour.Position, gridSize.x)] = neighbour;

                        if (!openSetContainsNeighbour)
                            openSet.Add(neighbour);
                    }
                }
            }

            result = default;
        }

        #endregion

        #region Méthodes privées

        /// <summary>
        /// Obtient la position de la node dans la liste
        /// </summary>
        /// <param name="pos">Position</param>
        /// <param name="gridSizeX">Largeur de la grille</param>
        [BurstCompile]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int ToIndex(in int2 pos, in int gridSizeX)
        {
            return pos.x + pos.y * gridSizeX;
        }

        /// <summary>
        /// Obtient les voisins de la cellule renseignée
        /// </summary>
        /// <param name="currentNodePos">Cellule de départ</param>
        /// <param name="grid">Grille de cellules</param>
        /// <param name="gridSize">Dimensions de la grille</param>
        /// <param name="result">Les voisins trouvés</param>
        [BurstCompile]
        private static void GetNeighbours(in int2 currentNodePos, in NativeArray<AStarNode> grid, in int2 gridSize, out NativeArray<AStarNode> result)
        {
            NativeList<AStarNode> neighbours = new(4, Allocator.Temp);

            int2 up = currentNodePos + new int2(0, 1);
            int2 right = currentNodePos + new int2(1, 0);
            int2 down = currentNodePos + new int2(0, -1);
            int2 left = currentNodePos + new int2(-1, 0);

            if (up.x > 0 && up.x < gridSize.x - 1 && up.y > 0 && up.y < gridSize.y - 1)
                neighbours.Add(grid[ToIndex(up, gridSize.x)]);

            if (right.x > 0 && right.x < gridSize.x - 1 && right.y > 0 && right.y < gridSize.y - 1)
                neighbours.Add(grid[ToIndex(right, gridSize.x)]);

            if (down.x > 0 && down.x < gridSize.x - 1 && down.y > 0 && down.y < gridSize.y - 1)
                neighbours.Add(grid[ToIndex(down, gridSize.x)]);

            if (left.x > 0 && left.x < gridSize.x - 1 && left.y > 0 && left.y < gridSize.y - 1)
                neighbours.Add(grid[ToIndex(left, gridSize.x)]);

            result = neighbours.AsArray();
        }

        /// <summary>
        /// Obtient le coût du mouvement entre les positions
        /// </summary>
        /// <param name="a">Position de la node actuelle</param>
        /// <param name="b">Position du voison</param> 
        [BurstCompile]
        private static float GetDistance(in int2 a, in int2 b)
        {
            int dstX = math.abs(a.x - b.x);
            int dstY = math.abs(a.y - b.y);

            if (dstX > dstY)
                return dstY + (dstX - dstY) * MOVEMENT_COST;

            return dstX + (dstY - dstX) * MOVEMENT_COST;
        }

        /// <summary>
        /// Obtient le chemin de cellules à suivre
        /// </summary>
        /// <param name="grid">Grille de cellules</param>
        /// <param name="gridSize">Dimensions de la grille</param>
        /// <param name="startPos">Départ</param>
        /// <param name="endPos">Fin</param>
        /// <param name="result">Liste de positions du chemin</param>
        [BurstCompile]
        private static void RetracePath(in NativeArray<AStarNode> grid, in int2 gridSize, in int2 startPos, in int2 endPos, out NativeArray<int2> result)
        {
            NativeList<int2> path = new(0, Allocator.Temp);
            AStarNode currentNode = grid[ToIndex(endPos, gridSize.x)];
            path.Add(currentNode.Position);

            while (currentNode.Position.x != startPos.x || currentNode.Position.y != startPos.y)
            {
                currentNode = grid[ToIndex(currentNode.ParentPos, gridSize.x)];
                path.Add(currentNode.Position);
            }

            path.Reverse();
            result = path.AsArray();
        }

        #endregion
    }
}