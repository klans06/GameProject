using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using Raylib_cs;

namespace GameProject.Source;

public class Player(int posX, int posY, int speed, float playerScale, PlayerState state, AssetManager assetManager, Map map)
{
    public int PositionX { get; private set; } = posX;
    public int PositionY { get; private set; } = posY;
    public int Speed { get; private set; } = speed;
    private float PlayerScale { get; set; } = playerScale;
    public Vector2 PlayerPosition;
    public PlayerState State { get; private set; } = state;
    private AssetManager AssetManager { get; } = assetManager;
    private Map GameMap { get; } = map;
    public Texture2D PlayerObject;

    public void InitializePlayer()
    {
        PlayerObject = AssetManager.GetPlayerTexture(State);
        PlayerPosition = Vector2.Create(PositionX, PositionY);
        
        // Raylib.DrawTexture(texture, PositionX, PositionY, Color.RayWhite);
        Raylib.DrawTextureEx(PlayerObject, PlayerPosition, 360f, PlayerScale, Color.RayWhite);
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
            // Skin = Skin switch
            // {
            //     PlayerSkin.WalkingLeft => PlayerSkin.IdleRight,
            //     PlayerSkin.WalkingRight => PlayerSkin.IdleLeft,
            //     PlayerSkin.WalkingForward => PlayerSkin.IdleForward,
            //     PlayerSkin.WalkingBackward => PlayerSkin.IdleBackward,
            //     _ => PlayerSkin.Default
            // };
        }
    }

    public void PaintPlayer()
    {
        // Texture2D texture = AssetManager.GetPlayerTexture(State);
        // Vector2 position = new Vector2(PositionX, PositionY);
        PlayerObject = AssetManager.GetPlayerTexture(State);
        PlayerPosition = Vector2.Create(PositionX, PositionY);
        
        // Raylib.DrawTexture(texture, PositionX, PositionY, Color.RayWhite);
        Raylib.DrawTextureEx(PlayerObject, PlayerPosition, 360f, PlayerScale, Color.RayWhite);
    }
}