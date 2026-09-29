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

    private int _currentFrameIndex;
    private int _framesSinceLastAnimationChange;
    private readonly int _animationCount = 5;
    private const int FramesBetweenAnimations = 2;
    
    private PlayerState State { get; set; } = state;
    private AssetManager AssetManager { get; } = assetManager;
    private Map GameMap { get; } = map;
    public Rectangle PlayerObject;
    private Vector2 PlayerPosition;
    
    /// <summary>
    /// Resolves a player state to its sprite atlas name or animation prefix.
    /// </summary>
    /// <param name="state">The idle or directional walking state to resolve.</param>
    /// <returns>The idle sprite name or a walking prefix, defaulting to the downward walking prefix for unknown states.</returns>
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

    /// <summary>
    /// Processes WASD input, checks map collisions, and updates the position and animation state.
    /// </summary>
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

    /// <summary>
    /// Advances the animation frame and draws the current walking or idle sprite at the player position.
    /// </summary>
    public void PaintPlayer()
    {
        string statePrefix = GetAnimationPrefix(State);
        
        _framesSinceLastAnimationChange++;
        if(_framesSinceLastAnimationChange >= FramesBetweenAnimations){
            _framesSinceLastAnimationChange = 0;
            _currentFrameIndex++;
            if(_currentFrameIndex > _animationCount)
            {
                _currentFrameIndex = 0;
            }
        }
        
        PlayerObject = State != PlayerState.Default
            ? AssetManager.GetPlayerRec(statePrefix + _currentFrameIndex)
            : AssetManager.GetPlayerRec(statePrefix);
        PlayerPosition = Vector2.Create(PositionX, PositionY);
        Rectangle textureRec = new Rectangle(PlayerPosition, PlayerObject.Width * PlayerScale,
            PlayerObject.Height * PlayerScale);
        Raylib.DrawTexturePro(AssetManager.PlayerTexture, PlayerObject, textureRec, Vector2.Zero, 0f, Color.RayWhite);
        // Raylib.DrawTextureRec(AssetManager.PlayerTexture, PlayerObject, PlayerPosition, Color.RayWhite);
    }
}