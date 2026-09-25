using Raylib_cs;

namespace GameProject.Source;

internal static class Program
{
    const int Columns = 45;
    const int Rows = 68;
    const int TileSize = 16;
    private const int TargetFps = 60;
    
    public static void Main()
    {
        AssetManager assetManager = new AssetManager();
        Map gameMap = new Map(Columns, Rows, assetManager, TileSize);
        
        Raylib.InitWindow(1080, 720, "GameProject");
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

