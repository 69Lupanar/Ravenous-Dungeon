using System;
using Assets.Scripts.TilePalettes;

namespace Assets.Scripts.MapGeneration
{
    /// <summary>
    /// R�sultat de l'événement
    /// </summary>
    public class GenerationEndedEventArgs : EventArgs
    {
        #region Propri�t�s

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