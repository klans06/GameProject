using System.Numerics;
using Overtile.Source.Core;
using Raylib_cs;

using Overtile.Source.World;
using Overtile.Source.Graphics;

namespace Overtile.Source.Entities;

public class Player(int posX, int posY, int speed, float playerScale, PlayerState state, AssetManager assetManager, Map map)
{
    public int PositionX { get; private set; } = posX;
    public int PositionY { get; private set; } = posY;
    private int Speed { get; set; } = speed;
    private float PlayerScale { get; set; } = playerScale;
    
    private int _currentFrame = 0;
    private float animationTimer = 0f;
    private const float FrameDuration = 0.135f;
    
    private PlayerState State { get; set; } = state;
    private AssetManager AssetManager { get; } = assetManager;
    private Map GameMap { get; } = map;
    public Rectangle PlayerObject;
    private Vector2 PlayerPosition;
    
    private string GetAnimationPrefix(PlayerState state)
    {
        return state switch
        {
            PlayerState.Default => "default",
            PlayerState.WalkingDown => "walk_down_",
            PlayerState.WalkingUp => "walk_up_",
            PlayerState.WalkingLeft => "walk_left_",
            PlayerState.WalkingRight => "walk_right_",
            _ => "walk_down_"
        };
    }

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
                State = PlayerState.WalkingUp;
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
                State = PlayerState.WalkingDown;
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
                State = PlayerState.WalkingLeft;
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
                State = PlayerState.WalkingRight;
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
        string statePrefix = GetAnimationPrefix(State);

        if (State != PlayerState.Default)
        {
            PlayerObject = AssetManager.GetPlayerRec(statePrefix + Raylib.GetFrameTime());
        }
        else
        {
            PlayerObject = AssetManager.GetPlayerRec(statePrefix);
        }

        PlayerPosition = Vector2.Create(PositionX, PositionY);

        Raylib.DrawTextureRec(AssetManager.PlayerTexture, PlayerObject, PlayerPosition, Color.RayWhite);
    }
}