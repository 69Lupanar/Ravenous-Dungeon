using System;

namespace Assets.Scripts.Runtime.Models.Tiles
{
    /// <summary>
    /// Représente les attributs d'une case
    /// </summary>
    [Flags]
    public enum EnvironmentTileLayerMask
    {
        Ground = 1,
        Wall = 2,
        Water = 8,
    }
}