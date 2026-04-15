using Assets.Scripts.Runtime.Models.Tiles;
using Assets.Scripts.Runtime.Models.ValueTypes;

namespace Assets.Scripts.Runtime.Models.Actors
{
    /// <summary>
    /// Données d'une case représentant une case interactive
    /// </summary>
    public struct InteractableActor : IActor<InteractableTileSO>, IInteractableActor
    {
        /// <summary>
        /// La case source
        /// </summary>
        public InteractableTileSO Data { get; set; }

        /// <summary>
        /// Les attributs de l'acteur
        /// </summary>
        public TileAttributes Attributes { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="data">La donnée</param>
        public InteractableActor(InteractableTileSO data) : this()
        {
            Data = data;
            this.Attributes = Attributes;
        }
    }
}