using System;
using Unity.Burst;
using Unity.Mathematics;

namespace Assets.Scripts.Runtime.ViewModels.Generation.LiquidGeneration
{
    /// <summary>
    /// Noeud utilisé par l'algorithme A*
    /// </summary>
    public struct AStarNode : IEquatable<AStarNode>
    {
        /// <summary>
        /// Position du noeud
        /// </summary>
        public readonly int2 Position { get; }

        /// <summary>
        /// Position du noeud parent
        /// </summary>
        public int2 ParentPos { get; set; }

        /// <summary>
        /// Distance du noeud par rapport au point de départ
        /// </summary>
        public float GCost { get; set; }

        /// <summary>
        /// Distance du noeud par rapport au point d'arrivée
        /// </summary>
        public float HCost { get; set; }

        /// <summary>
        /// Somme du GCost et du HCost
        /// </summary>
        public readonly float FCost => GCost + HCost;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="position">La position du noeud sur la grille</param>
        public AStarNode(int2 position)
        {
            Position = position;
            ParentPos = int2.zero;
            GCost = 0f;
            HCost = 0f;
        }

        /// <summary>
        /// Indique si les deux objets sont égaux
        /// </summary>
        /// <param name="other">L'objet comparé</param>
        [BurstCompile]
        public readonly bool Equals(AStarNode other)
        {
            return math.all(Position == other.Position);
        }
    }
}