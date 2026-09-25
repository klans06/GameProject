using Raylib_cs;

namespace GameProject.Source;

internal class Program
{
    const int Columns = 25;
    const int Rows = 20;
    const int TileSize = 32;
    private const int TargetFps = 60;
    
    public static void Main()
    {
        Map GameMap = new Map(Columns, Rows, TileSize);
        Raylib.InitWindow(800, 480, "Hello, World");
        Raylib.SetTargetFPS(TargetFps);

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            GameMap.DrawTile();
            
            Raylib.EndDrawing();
        }
        
        Raylib.CloseWindow();
    }
}

