using Overtile.Source.Entities;

namespace Overtile.Source.World;

public enum TileType
{
    Grass,
    GrassWFlower,
    GrassWFlowerPatch,
    Dirt,
    StoneWallBorder
}

public struct Tile
{
    public TileType Type { get; set; }
    public bool ShouldCollide { get; set; }
    // Item, loot support later
}