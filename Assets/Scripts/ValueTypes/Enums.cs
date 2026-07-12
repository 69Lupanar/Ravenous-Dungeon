using System;

namespace Assets.Scripts.ValueTypes
{
    /// <summary>
    /// Représente le layermask d'une case.
    /// Utilisé pour le déplacement des entités
    /// </summary>
    [Flags]
    public enum EnvironmentTileLayerMask
    {
        Ground = 1,
        Wall = 2,
    }

    /// <summary>
    /// Représente les attributs d'une case
    /// </summary>
    [Flags]
    public enum TileAttributes
    {
        SeeThrough = 1,
        Indestructible = 2,
    }

}