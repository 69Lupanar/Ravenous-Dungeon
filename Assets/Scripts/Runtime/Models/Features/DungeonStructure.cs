using System;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Features
{
    /// <summary>
    /// Représente une structure créée lors de la génération
    /// (salles, corridors, etc)
    /// </summary>
    [Serializable]
    public readonly struct DungeonStructure
    {
        #region Propriétés

        /// <summary>
        /// Le centre de la salle. Le résultat est inexact pour les salles de taille paire,
        /// vu qu'elles n'ont techniquement pas de centre.
        /// </summary>
        public readonly int2 Centroid
        {
            get
            {
                return new int2(Mathf.RoundToInt(Position.x + Dimensions.x / 2f), Mathf.RoundToInt(Position.y + Dimensions.y / 2f));
            }
        }

        #endregion

        #region Variables d'instance

        /// <summary>
        /// La position de la structure sur la carte
        /// </summary>
        [field: SerializeField]
        public readonly int2 Position { get; }

        /// <summary>
        /// Les dimensions de la structure
        /// </summary>
        [field: SerializeField]
        public readonly int2 Dimensions { get; }

        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="position">La position de la structure sur la carte</param>
        /// <param name="dimensions">Les dimensions de la structure</param>
        public DungeonStructure(int2 position, int2 dimensions)
        {
            Position = position;
            Dimensions = dimensions;
        }
        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Indique si les deux structures se superposent
        /// </summary>
        /// <param name="other">La structure voisine</param>
        /// <returns>true si les deux structures se superposent</returns>
        public bool Overlaps(DungeonStructure other)
        {
            int2 bottomLeft = Position - 1;
            int2 topRight = Position + Dimensions + 1;

            int2 otherBottomLeft = other.Position - 1;
            int2 otherBottomRight = otherBottomLeft + new int2(other.Dimensions.x, 0);
            int2 otherTopLeft = otherBottomLeft + new int2(0, other.Dimensions.y);
            int2 otherTopRight = other.Position + other.Dimensions + 1;

            bool containsBottomLeft = otherBottomLeft.x >= bottomLeft.x && otherBottomLeft.x <= topRight.x &&
                                      otherBottomLeft.y >= bottomLeft.y && otherBottomLeft.y <= topRight.y;

            bool containsBottomRight = otherBottomRight.x >= bottomLeft.x && otherBottomRight.x <= topRight.x &&
                                      otherBottomRight.y >= bottomLeft.y && otherBottomRight.y <= topRight.y;

            bool containsTopLeft = otherTopLeft.x >= bottomLeft.x && otherTopLeft.x <= topRight.x &&
                                      otherTopLeft.y >= bottomLeft.y && otherTopLeft.y <= topRight.y;

            bool containsTopRight = otherTopRight.x >= bottomLeft.x && otherTopRight.x <= topRight.x &&
                                      otherTopRight.y >= bottomLeft.y && otherTopRight.y <= topRight.y;

            bool topEdgeOverlaps = otherTopLeft.x < bottomLeft.x && otherTopRight.x > topRight.x &&
                                   otherTopLeft.y >= bottomLeft.y && otherTopLeft.y <= topRight.y;

            bool bottomEdgeOverlaps = otherBottomLeft.x < bottomLeft.x && otherBottomRight.x > topRight.x &&
                                      otherBottomLeft.y >= bottomLeft.y && otherBottomLeft.y <= topRight.y;

            bool leftEdgeOverlaps = otherBottomLeft.y < bottomLeft.y && otherTopLeft.y > topRight.y &&
                                      otherBottomLeft.x >= bottomLeft.x && otherBottomLeft.x <= topRight.x;

            bool rightEdgeOverlaps = otherBottomRight.y < bottomLeft.y && otherTopRight.y > topRight.y &&
                                      otherBottomRight.x >= bottomLeft.x && otherBottomRight.x <= topRight.x;

            return containsBottomLeft || containsBottomRight || containsTopLeft || containsTopRight ||
                   topEdgeOverlaps || bottomEdgeOverlaps || leftEdgeOverlaps || rightEdgeOverlaps;
        }

        #endregion
    }
}