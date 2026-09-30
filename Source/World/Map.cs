using System.Data;
using System.Numerics;
using Raylib_cs;

using Overtile.Source.Graphics;

namespace Overtile.Source.World;

public class Map
{
    private Tile[,] _tiles;
    public int TileSize { get; }
    public float TileScale { get; }
    public int Columns { get; }
    public int Rows { get; }
    private AssetManager AssetManager { get; }

    public Map(int columns, int rows, AssetManager assetManager, int tileSize = 32, float tileScale = 2.0f)
    {
        Columns = columns;
        Rows = rows;
        TileSize = tileSize;
        TileScale = tileScale;
        AssetManager = assetManager;
        _tiles = new Tile[rows, columns];
        
        InitializeMap();
    }

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