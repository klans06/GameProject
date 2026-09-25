using Raylib_cs;

namespace GameProject.Source;

internal static class Program
{
    const int Columns = 20;
    const int Rows = 20;
    const int TileSize = 32;
    private const int TargetFps = 60;
    
    public static void Main()
    {
        AssetManager assetManager = new AssetManager();
        Map gameMap = new Map(Columns, Rows, TileSize);
        
        Raylib.InitWindow(1080, 720, "Hello, World");
        Raylib.SetTargetFPS(TargetFps);
        assetManager.LoadContent();

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            gameMap.DrawTile();
            
            Raylib.EndDrawing();
        }
        
        Raylib.CloseWindow();
        assetManager.UnloadContent();
    }
}

