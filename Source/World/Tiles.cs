using Overtile.Source.Entities;

namespace Overtile.Source.World;

public enum TileType
{
    Grass,
    GrassWeed,
    GrassWFlowers,
    Shrub,
    StoneWallBorder
}

public struct Tile
{
    public TileType Type { get; set; }
    public bool ShouldCollide { get; set; }
    // Item, loot support later
}