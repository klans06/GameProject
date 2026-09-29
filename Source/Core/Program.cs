using Raylib_cs;

using Overtile.Source.Graphics;
using Overtile.Source.Entities;
using Overtile.Source.World;

namespace Overtile.Source.Core;

internal static class Program
{
    // Map Tiles
    const int Columns = 100;
    const int Rows = 100;
    const int TileSize = 24;
    
    // Game
    private const string Title = "Overtile";
    private const int ScreenWidth = 1080;
    private const int ScreenHeight = 720;
    private const int TargetFps = 60;
    
    // Player
    private const int PlayerSpawnPositionX = Rows / 2 * TileSize;
    private const int PlayerSpawnPositionY = Columns / 2 * TileSize;
    private const int PlayerSpawnSpeed = 2;
    private const float PlayerScale = 1.5f;
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
        PlayerCamera playerCamera = new PlayerCamera(player, gameMap, ScreenHeight, ScreenWidth);
        
        Raylib.InitWindow(ScreenWidth, ScreenHeight, Title);
        Raylib.SetTargetFPS(TargetFps);
        assetManager.LoadMapContent();
        assetManager.LoadPlayerContent();
        playerCamera.InitializePlayerCamera();

        while (!Raylib.WindowShouldClose())
        {
            player.UpdatePosition();
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.White);
                    Raylib.BeginMode2D(playerCamera.Camera);
                        gameMap.DrawTile();
                        player.PaintPlayer();
                        playerCamera.UpdatePosition();
                    Raylib.EndMode2D();
                Raylib.EndDrawing();
        }
        
        assetManager.UnloadPlayerContent();
        Raylib.CloseWindow();
        assetManager.UnloadMapContent();
    }
}

