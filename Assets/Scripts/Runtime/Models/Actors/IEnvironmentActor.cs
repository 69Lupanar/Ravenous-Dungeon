using Assets.Scripts.Runtime.Models.ValueTypes;

namespace Assets.Scripts.Runtime.Models.Actors
{
    /// <summary>
    /// Propriétés des cases de type Environnement (murs, sol, liquides, etc.)
    /// </summary>
    public interface IEnvironmentActor
    {
        /// <summary>
        /// Les attributs de cette case
        /// </summary>
        public EnvironmentTileLayerMask LayerMask { get; set; }
    }
}