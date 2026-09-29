using System.Numerics;
using Raylib_cs;

using Overtile.Source.Entities;
using Overtile.Source.World;

namespace Overtile.Source.Core;

public class PlayerCamera(Player player, Map map, int screenHeight, int screenWidth, float rotation = 0f, float zoom = 1f)
{
    private Player Player { get; set; } = player;
    private Map GameMap { get; set; } = map;
    private float Rotation { get; set; } = rotation;
    private float Zoom { get; set; } = zoom;
    private int ScreenHeight { get; set; } = screenHeight;
    private int ScreenWidth { get; set; } = screenWidth;

    public Camera2D Camera;

    public void InitializePlayerCamera()
    {
        Camera.Offset = new Vector2(ScreenWidth / 2.0f, ScreenHeight / 2.0f);
        Camera.Target = GetClampedPosition();
        Camera.Rotation = Rotation;
        Camera.Zoom = Zoom;
    }

    public void UpdatePosition()
    {
        Camera.Target = GetClampedPosition();
    }

    // Calculate position to avoid camera peeking past border tiles
    // Has offset slightly towards the top right
    private Vector2 GetClampedPosition()
    {
        float playerCenterX = Player.PositionX + (Player.PlayerObject.Width / 2.0f);
        float playerCenterY = Player.PositionY + (Player.PlayerObject.Height / 2.0f);
        float totalMapWidth = GameMap.Columns * GameMap.TileSize;
        float totalMapHeight = GameMap.Rows * GameMap.TileSize;
        float halfScreenW = (ScreenWidth / 2.0f) / Zoom;
        float halfScreenH = (ScreenHeight / 2.0f) / Zoom;
        float clampedX = Math.Clamp(playerCenterX, halfScreenW, totalMapWidth - halfScreenW);
        float clampedY = Math.Clamp(playerCenterY, halfScreenH, totalMapHeight - halfScreenH);
        
        Vector2 clampedPosition = new Vector2(clampedX, clampedY);
        return clampedPosition;
    }
}