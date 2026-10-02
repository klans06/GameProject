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
    /// Creates a map with noise-based ground tiles and solid stone wall borders.
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
    
    /// <summary>
    /// Replaces the map's noise generator using the supplied seed or a randomly selected seed.
    /// </summary>
    /// <param name="seed">The seed to use, or null to choose a value from 0 through 99998.</param>
    private void InitializeNoise(int? seed)
    {
        int? mapSeed = seed ?? Random.Shared.Next(99999);
        if (mapSeed == null) {Console.WriteLine("Seed not generated");}
        
        Noise = new FastNoiseLite(mapSeed.Value);
        Console.WriteLine($"Seed is {mapSeed}");
    }

    /// <summary>
    /// Selects a ground tile from a noise sample and sets the frequency for subsequent noise samples to 0.25.
    /// </summary>
    /// <returns>
    /// GrassWeed for samples from 0.01 through 0.5, GrassWFlowers from 0.51 through 1,
    /// and Grass otherwise. Both ranges include their endpoints.
    /// </returns>
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

    /// <summary>
    /// Selects a decorative prop from a noise sample and sets the frequency for subsequent noise samples to 2.
    /// </summary>
    /// <returns>
    /// PropShrub for samples from 0.1 through 0.16, PropLongGrass from 0.171 through 0.182,
    /// PropMushrooms from 0.191 through 0.195, and None otherwise. All ranges include their endpoints.
    /// </returns>
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

    /// <summary>
    /// Retrieves the loaded texture for a ground tile, border, or decorative prop.
    /// </summary>
    /// <returns>The matching texture, or the stone wall border texture for None or an unrecognized tile type.</returns>
    /// <exception cref="KeyNotFoundException">The selected texture, including any fallback texture, has not been loaded.</exception>
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
    /// Fills the grid with collidable stone wall borders and non-collidable, noise-based ground tiles,
    /// storing each tile's noise sample and clearing its decorative prop.
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

                if (isBorder)
                {
                    t.Type = TileType.StoneWallBorder;
                    t.isMapBorder = true;
                } else
                {
                    t.Type = GetTileThreshold(noise);
                }
                
                t.ShouldCollide = isBorder;
                t.Noise = noise;

                _tiles[x, y] = t;
            }
        }
    }

    /// <summary>
    /// Draws each map tile at its grid position using the configured texture scale, then draws its decorative prop.
    /// </summary>
    /// <remarks>
    /// Recomputes and stores props on each call, including on border tiles, without changing collision flags.
    /// Prop selection sets the noise frequency to 2 for subsequent samples.
    /// </remarks>
    /// <exception cref="KeyNotFoundException">A required base tile or prop texture has not been loaded.</exception>
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

                if (_tiles[x, y].Prop != TileType.None && !_tiles[x, y].isMapBorder)
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