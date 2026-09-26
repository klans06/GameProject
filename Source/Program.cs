using Raylib_cs;

namespace GameProject.Source;

internal static class Program
{
    // Map Tiles
    const int Columns = 30;
    const int Rows = 45;
    const int TileSize = 24;
    
    // Game
    private const int TargetFps = 60;
    
    // Player
    private const int PlayerSpawnPositionX = Rows / 2 * TileSize;
    private const int PlayerSpawnPositionY = Columns / 2 * TileSize;
    private const int PlayerSpawnSpeed = 1;
    private const float PlayerScale = 0.4f;
    private const PlayerState PlayerSpawnState = PlayerState.Default;
    
    public static void Main()
    {
        AssetManager assetManager = new AssetManager();
        Map gameMap = new Map(Columns, Rows, assetManager, TileSize);
        Player player = new Player(
            PlayerSpawnPositionX,
            PlayerSpawnPositionY,
            PlayerSpawnSpeed,
            PlayerScale,
            PlayerSpawnState,
            assetManager,
            gameMap
        );
        
        Raylib.InitWindow(1080, 720, "GameProject");
        Raylib.SetTargetFPS(TargetFps);
        assetManager.LoadContent();

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            gameMap.DrawTile();
            player.InitializePlayer();
            // gameMap.DrawMapCollisions(player);
            player.UpdatePosition();
            
            Raylib.EndDrawing();
        }
        
        Raylib.CloseWindow();
        assetManager.UnloadContent();
    }
}

