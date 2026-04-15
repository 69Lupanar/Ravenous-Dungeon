using Assets.Scripts.Runtime.Models.Tiles;
using Assets.Scripts.Runtime.Models.ValueTypes;

namespace Assets.Scripts.Runtime.Models.Actors
{
    /// <summary>
    /// Acteur représentant une instance d'une case.
    /// Permet d'avoir des cases avec des variables de différentes valeurs
    /// mais basées sur la même donnée source
    /// </summary>
    /// <typeparam name="T">Le type de la case</typeparam>
    public interface IActor<T> where T : TileEntitySO
    {
        /// <summary>
        /// La case source
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Les attributs de l'acteur
        /// </summary>
        public TileAttributes Attributes { get; set; }
    }
}