using Assets.Scripts.Runtime.Models.Tiles;
using Assets.Scripts.Runtime.Models.ValueTypes;

namespace Assets.Scripts.Runtime.Models.Actors
{
    /// <summary>
    /// Données d'une case représentant une région liquide l'environnement (eau, lave etc.)
    /// </summary>
    public struct LiquidActor : IActor<LiquidTileSO>, IEnvironmentActor, ILiquidActor
    {
        /// <summary>
        /// La case source
        /// </summary>
        public LiquidTileSO Data { get; set; }

        /// <summary>
        /// Les attributs de cette case
        /// </summary>
        public EnvironmentTileLayerMask LayerMask { get; set; }

        /// <summary>
        /// La force du courant. Si un personnage remonte le courant,
        /// son temps de mouvement est multiplié par cette valeur.
        /// S'il descent le courant, il est divisé par cette valeur.
        /// </summary>
        public int CurrentStrength { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="data">La donnée</param>
        public LiquidActor(LiquidTileSO data) : this()
        {
            Data = data;
            this.LayerMask = data.LayerMask;
            this.CurrentStrength = data.CurrentStrength;
        }
    }
}