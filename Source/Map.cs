using System.Data;
using Raylib_cs;

namespace GameProject.Source;

public class Map
{
    private readonly ETileType[,] _tiles;
    public int TileSize { get; }
    public int Columns { get; }
    public int Rows { get; }

    public Map(int columns, int rows, int tileSize = 32)
    {
        Columns = columns;
        Rows = rows;
        TileSize = tileSize;
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
                
                int pixelX = x * TileSize;
                int pixelY = y * TileSize;

                if (x == 0 || x == Columns - 1 || y == 0 || y == Rows - 1)
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
                Color tileColor = _tiles[x, y] switch
                {
                    ETileType.Grass => Color.Green,
                    ETileType.StoneWallBorder => Color.DarkGray,
                    _ => Color.DarkGreen // Fallback color
                };
                
                int pixelX = x * TileSize;
                int pixelY = y * TileSize;
                
                Raylib.DrawRectangle(pixelX, pixelY, TileSize, TileSize, tileColor);
            }
        }
    }
}