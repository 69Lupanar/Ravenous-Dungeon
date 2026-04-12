namespace Assets.Scripts.Runtime.Models.Actors
{
    /// <summary>
    /// Données d'une case représentant un liquide (eau, lave, etc)
    /// </summary>
    public interface ILiquidActor
    {
        /// <summary>
        /// La force du courant. Si un personnage remonte le courant,
        /// son temps de mouvement est multiplié par cette valeur.
        /// S'il descent le courant, il est divisé par cette valeur.
        /// </summary>
        public int CurrentStrength { get; set; }
    }
}