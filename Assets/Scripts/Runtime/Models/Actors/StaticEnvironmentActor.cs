using Assets.Scripts.Runtime.Models.Tiles;
using Assets.Scripts.Runtime.Models.ValueTypes;
using UnityEngine;

namespace Assets.Scripts.Runtime.Models.Actors
{
    /// <summary>
    /// Données d'une case représentant un élément statique de l'environnement (mur, sol etc.)
    /// </summary>
    public struct StaticEnvironmentActor : IActor<StaticEnvironmentTileSO>, IEnvironmentActor
    {
        /// <summary>
        /// La case source
        /// </summary>
        public StaticEnvironmentTileSO Data { get; set; }

        /// <summary>
        /// Les attributs de cette case
        /// </summary>
        [field: SerializeField]
        public TileAttributes Attributes { get; set; }

        /// <summary>
        /// Les attributs de cette case
        /// </summary>
        [field: SerializeField]
        public EnvironmentTileLayerMask LayerMask { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="data">La donnée</param>
        public StaticEnvironmentActor(StaticEnvironmentTileSO data)
        {
            Data = data;
            Attributes = data.Attributes;
            LayerMask = data.LayerMask;
        }
    }
}