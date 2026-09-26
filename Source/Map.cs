using System.Data;
using System.Numerics;
using Raylib_cs;

namespace GameProject.Source;

public class Map
{
    private readonly Tile[,] _tiles;
    private int TileSize { get; }
    private int Columns { get; }
    private int Rows { get; }
    private AssetManager AssetManager { get; }

    public Map(int columns, int rows, AssetManager assetManager, int tileSize = 32)
    {
        Columns = columns;
        Rows = rows;
        TileSize = tileSize;
        AssetManager = assetManager;
        _tiles = new Tile[rows, columns];
        
        InitializeMap();
    }

    private void InitializeMap()
    {
        for (int x = 0; x < Rows; x++)
        {
            for (int y = 0; y < Columns; y++)
            {
                bool isBorder = (x == 0 || x == Rows - 1 || y == 0 || y == Columns - 1);

                _tiles[x, y] = new Tile
                {
                    Type = isBorder ? TileType.StoneWallBorder : TileType.Grass,
                    ShouldCollide = isBorder
                };

                // if (x == 0 || x == Rows - 1 || y == 0 || y == Columns - 1)
                // {
                //     _tiles[x, y].Type = TileType.StoneWallBorder;
                // }
                // else
                // {
                //     _tiles[x, y].Type = TileType.Grass;
                // }
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
                    TileType.StoneWallBorder => AssetManager.GetMapTexture(TileType.StoneWallBorder),
                    _ => AssetManager.GetMapTexture(TileType.StoneWallBorder) // Fallback color
                };
                
                int pixelX = x * TileSize;
                int pixelY = y * TileSize;

                Vector2 tilePosition = new Vector2(pixelX, pixelY);
                
                Rectangle tileOutline = new Rectangle(pixelX, pixelY, TileSize, TileSize);
                Raylib.DrawTextureEx(tileTexture, tilePosition, 360f, TileSize,Color.RayWhite);
                Raylib.DrawRectangleLinesEx(tileOutline, 0.5f, Color.Black);
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