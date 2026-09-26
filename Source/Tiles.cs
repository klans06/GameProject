namespace GameProject.Source;

public enum TileType
{
    Grass, // 0
    GrassWFlower,
    GrassWFlowerPatch,
    Dirt,
    StoneWallBorder // 4
}

public struct Tile
{
    public TileType Type { get; set; }
    public bool ShouldCollide { get; set; }
    // Item, loot support later
}
//
// public static readonly TileDefinition[] Definitions =
// [
//     new TileDefinition("Grass"),
//     new TileDefinition("Wall", shouldCollide : true)
// ];

public enum PlayerState
{
    Default, // 0
    WalkingDown,
    WalkingUp,
    WalkingLeft,
    WalkingRight
    // Walk0,
    // Walk1,
    // Walk2,
    // Walk3,
    // Walk4,
    // Walk5,
    // Walk6,
    // Walk7 // 6
}

public enum BuildingTile
{
    
}