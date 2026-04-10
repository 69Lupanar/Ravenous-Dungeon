using System;
using Assets.Scripts.Runtime.Models.Map;
using Assets.Scripts.Runtime.Models.Tiles.TilePalette;

namespace Assets.Scripts.Runtime.Models.Generation
{
    /// <summary>
    /// Résultat de l'événement
    /// </summary>
    public class GenerationEndedEventArgs : EventArgs
    {
        #region Propriétés

        /// <summary>
        /// La grille des cases créées
        /// </summary>
        public Grid Grid { get; }

        /// <summary>
        /// Contient les sprites utilisés pour l'affichage des cases
        /// </summary>
        public SpriteLibrarySO SpriteLibrary { get; }

        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="grid">La grille des cases créées</param>
        /// <param name="spriteLibrary">Contient les sprites utilisés pour l'affichage des cases</param>
        public GenerationEndedEventArgs(Grid grid, SpriteLibrarySO spriteLibrary)
        {
            Grid = grid;
            SpriteLibrary = spriteLibrary;
        }

        #endregion
    }
}