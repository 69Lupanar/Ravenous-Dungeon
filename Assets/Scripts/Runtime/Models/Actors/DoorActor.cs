using Assets.Scripts.Runtime.Models.Tiles;
using Assets.Scripts.Runtime.Models.ValueTypes;

namespace Assets.Scripts.Runtime.Models.Actors
{
    /// <summary>
    /// Données d'une case représentant une porte
    /// </summary>
    public struct DoorActor : IActor<DoorTileSO>, IInteractableActor
    {
        /// <summary>
        /// La case source
        /// </summary>
        public DoorTileSO Data { get; set; }

        /// <summary>
        /// true si la porte est ouverte
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Les attributs de la porte. Permet de permuter 
        /// l'état SeeThrough
        /// </summary>
        public TileAttributes Attributes { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="data">La donnée</param>
        public DoorActor(DoorTileSO data) : this()
        {
            Data = data;
            this.IsOpen = false;
            this.Attributes = data.Attributes;
        }
    }
}