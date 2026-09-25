using System.Data;
using Raylib_cs;

namespace GameProject.Source;

public class Map
{
    private readonly ETileType[,] _tiles;
    private int TileSize { get; }
    private int Columns { get; }
    private int Rows { get; }
    private AssetManager TileManager { get; }

    public Map(int columns, int rows, AssetManager assetManager, int tileSize = 32)
    {
        Columns = columns;
        Rows = rows;
        TileSize = tileSize;
        TileManager = assetManager;
        _tiles = new ETileType[rows, columns];
        
        InitializeMap();
    }

    private void InitializeMap()
    {
        for (int x = 0; x < Rows; x++)
        {
            for (int y = 0; y < Columns; y++)
            {
                ETileType currentTile = _tiles[x, y];

                if (x == 0 || x == Rows - 1 || y == 0 || y == Columns - 1)
                {
                    _tiles[x, y] = ETileType.StoneWallBorder;
                }
                else
                {
                    _tiles[x, y] = ETileType.Grass;
                }
            }
        }
    }

    public void DrawTile()
    {
        for (int x = 0; x < Rows; x++)
        {
            for (int y = 0; y < Columns; y++)
            {
                Texture2D tileTexture = _tiles[x, y] switch
                {
                    ETileType.Grass => TileManager.GetTexture(ETileType.Grass),
                    ETileType.StoneWallBorder => TileManager.GetTexture(ETileType.StoneWallBorder),
                    _ => TileManager.GetTexture(ETileType.StoneWallBorder) // Fallback color
                };
                
                int pixelX = x * TileSize;
                int pixelY = y * TileSize;
                
                Raylib.DrawTexture(tileTexture, pixelX, pixelY, Color.RayWhite);
                Raylib.DrawRectangleLines(pixelX, pixelY, TileSize, TileSize, Color.Black);
            }
        }
    }
}