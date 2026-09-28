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

public enum PlayerState
{
    Default,
    WalkingDown,
    WalkingUp,
    WalkingLeft,
    WalkingRight
    // For walking animation sprite
    
    // Walk0,
    // Walk1,
    // Walk2,
    // Walk3,
    // Walk4,
    // Walk5,
    // Walk6,
    // Walk7
}