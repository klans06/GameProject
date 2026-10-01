using System.Data;
using System.IO.Pipes;
using System.Numerics;
using Raylib_cs;

using Overtile.Source.Graphics;
using Overtile.Source.World.Generation;

namespace Overtile.Source.World;

public class Map
{
    private readonly Tile[,] _tiles;
    private AssetManager AssetManager { get; }
    private FastNoiseLite  Noise { get; set; }
    private float TileScale { get; }
    public int TileSize { get; }
    public int Columns { get; }
    public int Rows { get; }

    /// <summary>
    /// Creates a map with randomized ground tiles and solid stone wall borders.
    /// </summary>
    /// <param name="columns">The number of columns in the tile grid.</param>
    /// <param name="rows">The number of rows in the tile grid.</param>
    /// <param name="assetManager">The asset manager that supplies map textures for rendering.</param>
    /// <param name="seed">The seed used to generate the tile texture placement. Null for random seed generation.</param>
    /// <param name="tileSize">The grid spacing in pixels used for placement and collision checks.</param>
    /// <param name="tileScale">The scale applied to tile textures when rendering.</param>
    public Map(int columns, int rows, AssetManager assetManager, int? seed = null, int tileSize = 32, float tileScale = 1.5f)
    {
        Columns = columns;
        Rows = rows;
        TileSize = tileSize;
        TileScale = tileScale;
        AssetManager = assetManager;
        _tiles = new Tile[rows, columns];
        
        InitializeNoise(seed);
        InitializeMap();
    }
    
    private void InitializeNoise(int? seed)
    {
        int? mapSeed = seed ?? Random.Shared.Next(99999);
        if (mapSeed == null) {Console.WriteLine("Seed not generated");}
        
        Noise = new FastNoiseLite(mapSeed.Value);
        Console.WriteLine($"Seed is {mapSeed}");
    }

    private TileType GetTileThreshold(float noise)
    {
        Noise.SetFrequency(0.25f);
        if (noise is >= -1.0f and <= 0f)
        {
            return TileType.Grass;
        } else if (noise is >= 0.01f and <= 0.5f)
        {
            return TileType.GrassWeed;
        } else if (noise is >= 0.51f and <= 1f)
        {
            return TileType.GrassWFlowers;
        } else
        {
            return TileType.Grass;
        }
    }

    private TileType GetPropThreshold(float noise)
    {
        Noise.SetFrequency(2f);
        if (noise is >= 0.1f and <= 0.16f)
        {
            return TileType.PropShrub;
        } else if (noise is >= 0.171f and <= 0.182f)
        {
            return TileType.PropLongGrass;
        } else if (noise is >= 0.191f and <= 0.195f)
        {
            return TileType.PropMushrooms;
        } else
        {
            return TileType.None;
        }
    }

    private Texture2D GetTileTexture(TileType tile)
    {
        Texture2D tileTexture = tile switch
        {
            TileType.Grass => AssetManager.GetMapTexture(TileType.Grass),
            TileType.GrassWeed => AssetManager.GetMapTexture(TileType.GrassWeed),
            TileType.GrassWFlowers => AssetManager.GetMapTexture(TileType.GrassWFlowers),
            TileType.StoneWallBorder => AssetManager.GetMapTexture(TileType.StoneWallBorder),
            TileType.PropShrub => AssetManager.GetMapTexture(TileType.PropShrub),
            TileType.PropLongGrass => AssetManager.GetMapTexture(TileType.PropLongGrass),
            TileType.PropMushrooms => AssetManager.GetMapTexture(TileType.PropMushrooms),
            _ => AssetManager.GetMapTexture(TileType.StoneWallBorder) // Fallback color
        };

        return tileTexture;
    }

    /// <summary>
    /// Fills the grid with collidable stone wall borders and non-collidable, randomly selected ground tiles.
    /// </summary>
    private void InitializeMap()
    {
        for (int x = 0; x < Rows; x++)
        {
            for (int y = 0; y < Columns; y++)
            {
                bool isBorder = (x == 0 || x == Rows - 1 || y == 0 || y == Columns - 1);

                var t = new Tile();
                
                float noise = Noise.GetNoise(x, y);

                t.Type = isBorder ? TileType.StoneWallBorder : GetTileThreshold(noise);
                t.Prop = TileType.None;
                t.ShouldCollide = isBorder;
                t.Noise = noise;

                _tiles[x, y] = t;
            }
        }
    }

    public void DrawBaseMapTiles()
    {
        for (int x = 0; x < Rows; x++)
        {
            for (int y = 0; y < Columns; y++)
            {
                Texture2D tileTexture = GetTileTexture(_tiles[x, y].Type);
                _tiles[x, y].Prop = GetPropThreshold(Noise.GetNoise(x, y));
                
                int pixelX = x * TileSize;
                int pixelY = y * TileSize;

                Vector2 tilePosition = new Vector2(pixelX, pixelY);
                // Rectangle tileOutline = new Rectangle(pixelX, pixelY, TileSize, TileSize);
                Raylib.DrawTextureEx(tileTexture, tilePosition, 360f, TileScale,Color.RayWhite);

                if (_tiles[x, y].Prop != TileType.None)
                {
                    Texture2D propTexture = GetTileTexture(_tiles[x, y].Prop);
                    Raylib.DrawTextureEx(propTexture, tilePosition, 360f, TileScale, Color.RayWhite);
                }
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