using System;

namespace Assets.Scripts.Runtime.Models.ValueTypes
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
        Lava = 16,
    }

    /// <summary>
    /// Le type d'une rivière pouvant être générée
    /// </summary>
    public enum RiverType
    {
        Water = 1,
        Lava = 2,
    }
}