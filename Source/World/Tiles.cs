using Overtile.Source.Entities;

namespace Overtile.Source.World;

public enum TileType
{
    None,
    Grass,
    GrassWeed,
    GrassWFlowers,
    PropShrub,
    PropLongGrass,
    PropMushrooms,
    StoneWallBorder
}

public class Tile
{
    public TileType Type { get; set; }
    public TileType Prop { get; set; }
    public bool isMapBorder { get; set; }
    public bool ShouldCollide { get; set; }
    public float Noise { get; set; }
    // Item, loot support later
}

public enum MiscTextureType
{
    Cursor
}