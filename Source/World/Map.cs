using System.Data;
using System.Numerics;
using Raylib_cs;

using Overtile.Source.Graphics;

namespace Overtile.Source.World;

public class Map
{
    private readonly Tile[,] _tiles;
    private AssetManager AssetManager { get; }
    private float TileScale { get; }
    private int? _seed;
    public int TileSize { get; }
    public int Columns { get; }
    public int Rows { get; }

    /// <summary>
    /// Creates a map with randomized ground tiles and solid stone wall borders.
    /// </summary>
    /// <param name="columns">The number of columns in the tile grid.</param>
    /// <param name="rows">The number of rows in the tile grid.</param>
    /// <param name="assetManager">The asset manager that supplies map textures for rendering.</param>
    /// <param name="seed">The seed used to generate the tile texture placement.</param>
    /// <param name="tileSize">The grid spacing in pixels used for placement and collision checks.</param>
    /// <param name="tileScale">The scale applied to tile textures when rendering.</param>
    public Map(int columns, int rows, AssetManager assetManager, int? seed, int tileSize = 32, float tileScale = 1.5f)
    {
        Columns = columns;
        Rows = rows;
        TileSize = tileSize;
        TileScale = tileScale;
        AssetManager = assetManager;
        _tiles = new Tile[rows, columns];
        _seed = seed ?? Random.Shared.Next(999999999);
        
        Console.WriteLine($"Seed is {_seed}");
        InitializeMap();
    }

    /// <summary>
    /// Fills the grid with collidable stone wall borders and noncollidable, randomly selected ground tiles.
    /// </summary>
    private void InitializeMap()
    {
        TileType PickGroundTile() =>
            Random.Shared.Next(100) switch
            {
                < 50 => TileType.Grass,
                < 65 => TileType.GrassWeed,
                < 95 => TileType.GrassWFlowers,
                _ => TileType.GrassWeed,
            };
        
        for (int x = 0; x < Rows; x++)
        {
            for (int y = 0; y < Columns; y++)
            {
                bool isBorder = (x == 0 || x == Rows - 1 || y == 0 || y == Columns - 1);

                var t = new Tile();

                t.Type = isBorder ? TileType.StoneWallBorder : PickGroundTile();
                t.ShouldCollide = isBorder;

                _tiles[x, y] = t;
            }
        }
    }
 
    /// <summary>
    /// Draws each map tile at its grid position using the configured texture scale.
    /// </summary>
    public void DrawTile()
    {
        for (int x = 0; x < Rows; x++)
        {
            for (int y = 0; y < Columns; y++)
            {
                Texture2D tileTexture = _tiles[x, y].Type switch
                {
                    TileType.Grass => AssetManager.GetMapTexture(TileType.Grass),
                    TileType.GrassWeed => AssetManager.GetMapTexture(TileType.GrassWeed),
                    TileType.GrassWFlowers => AssetManager.GetMapTexture(TileType.GrassWFlowers),
                    TileType.StoneWallBorder => AssetManager.GetMapTexture(TileType.StoneWallBorder),
                    _ => AssetManager.GetMapTexture(TileType.StoneWallBorder) // Fallback color
                };
                
                int pixelX = x * TileSize;
                int pixelY = y * TileSize;

                Vector2 tilePosition = new Vector2(pixelX, pixelY);
                
                Rectangle tileOutline = new Rectangle(pixelX, pixelY, TileSize, TileSize);
                Raylib.DrawTextureEx(tileTexture, tilePosition, 360f, TileScale,Color.RayWhite);
                // Raylib.DrawRectangleLinesEx(tileOutline, 0.5f, Color.Black);
            }
        }
    }

    public bool IsTileSolid(int pixelX, int pixelY)
    {
        int gridX = pixelX / TileSize;
        int gridY = pixelY / TileSize;

        if (gridX < 0 || gridX >= Rows || gridY < 0 || gridY >= Columns)
        {
            return true;
        }

        return _tiles[gridX, gridY].ShouldCollide;
    }
}