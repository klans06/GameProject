using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using Raylib_cs;

namespace Overtile.Source;

public class Player(int posX, int posY, int speed, float playerScale, PlayerState state, AssetManager assetManager, Map map)
{
    public int PositionX { get; private set; } = posX;
    public int PositionY { get; private set; } = posY;
    private int Speed { get; set; } = speed;
    private float PlayerScale { get; set; } = playerScale;
    private PlayerState State { get; set; } = state;
    private AssetManager AssetManager { get; } = assetManager;
    private Map GameMap { get; } = map;
    public Texture2D PlayerObject;
    private Vector2 PlayerPosition;

    public void UpdatePosition()
    {
        bool isMoving = false;
        if (Raylib.IsKeyDown(KeyboardKey.W))
        {
            int positionY = PositionY - Speed - 1;
            if (GameMap.IsTileSolid(PositionX, positionY))
            {
                isMoving = false;
            }
            else
            {
                PositionY -= Speed;
                State = PlayerState.Default;
                isMoving = true;
            }
        }

        if (Raylib.IsKeyDown(KeyboardKey.S))
        {
            int positionY = PositionY + Speed + 1;
            if (GameMap.IsTileSolid(PositionX, positionY))
            {
                isMoving = false;
            }
            else
            {
                PositionY += Speed;
                State = PlayerState.Default;
                isMoving = true;
            }
        }

        if (Raylib.IsKeyDown(KeyboardKey.A))
        {
            int positionX = PositionX - Speed - 1;
            if (GameMap.IsTileSolid(positionX, PositionY))
            {
                isMoving = false;
            }
            else
            {
                PositionX -= Speed;
                State = PlayerState.Default;
                isMoving = true;
            }
        }

        if (Raylib.IsKeyDown(KeyboardKey.D))
        {
            int positionX = PositionX + Speed + 1;
            if (GameMap.IsTileSolid(positionX, PositionY))
            {
                isMoving = false;
            }
            else
            {
                PositionX += Speed;
                State = PlayerState.Default;
                isMoving = true;
            }
        }

        if (!isMoving)
        {
            State = PlayerState.Default;
        }
    }

    public void PaintPlayer()
    {
        PlayerObject = AssetManager.GetPlayerTexture(State);
        PlayerPosition = Vector2.Create(PositionX, PositionY);
        
        Raylib.DrawTextureEx(PlayerObject, PlayerPosition, 360f, PlayerScale, Color.RayWhite);
    }
}