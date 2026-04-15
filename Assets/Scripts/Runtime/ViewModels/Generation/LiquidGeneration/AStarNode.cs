using System;
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
        public int GCost { get; set; }

        /// <summary>
        /// Distance du noeud par rapport au point d'arrivée
        /// </summary>
        public int HCost { get; set; }

        /// <summary>
        /// Somme du GCost et du HCost
        /// </summary>
        public readonly int FCost => GCost + HCost;

        /// <summary>
        /// Constructeur
        /// </summary>
        public AStarNode(int x, int y)
        {
            Position = new int2(x, y);
            ParentPos = int2.zero;
            GCost = 0;
            HCost = 0;
        }

        /// <summary>
        /// Indique si les deux objets sont égaux
        /// </summary>
        /// <param name="other">L'objet comparé</param>
        public readonly bool Equals(AStarNode other)
        {
            return math.all(Position == other.Position) && GCost == other.GCost && HCost == other.HCost && math.all(ParentPos == other.ParentPos);
        }
    }
}