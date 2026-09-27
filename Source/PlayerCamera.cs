using System.Numerics;
using Raylib_cs;

namespace GameProject.Source;

public class PlayerCamera(Player player, int screenHeight, int screenWidth, float rotation = 0f, float zoom = 1f)
{
    private Player Player { get; set; } = player;
    private float Rotation { get; set; } = rotation;
    private float Zoom { get; set; } = zoom;
    private int ScreenHeight { get; set; } = screenHeight;
    private int ScreenWidth { get; set; } = screenWidth;

    private Camera2D Camera;

    public void InitializePlayerCamera()
    {
        Camera.Offset = new Vector2(ScreenWidth / 2.0f, ScreenHeight / 2.0f);
        Camera.Target = new Vector2(Player.PositionX, Player.PositionY);
        Camera.Rotation = Rotation;
        Camera.Zoom = Zoom;
        
        Raylib.BeginMode2D(Camera);
    }

    public void UpdatePosition()
    {
        
    }
}