using System.Numerics;
using Overtile.Source.Graphics;
using Overtile.Source.World;
using Raylib_cs;

namespace Overtile.Source.Core;

public class Cursor
{
    private AssetManager AssetManager { get; }
    private Texture2D CursorTexture { get; }
    
    /// <summary>
    /// Initiates the game cursor texture, hides the default OS cursor and fetches the Asset Manager.
    /// </summary>
    public Cursor(AssetManager assetManager)
    {
        AssetManager = assetManager;
        CursorTexture = AssetManager.GetMiscTexture(MiscTextureType.Cursor);
        
        Raylib.HideCursor();
    }

    /// <summary>
    /// Fetches the current mouse position and draws the game's cursor in place of it.
    /// </summary>
    /// <remarks>Should be called after camera's EndMode2D otherwise will not display correctly.</remarks>
    public void UpdateCursor()
    {
        Vector2 mousePosition = Raylib.GetMousePosition();
        
        Raylib.DrawTexture(CursorTexture, (int)mousePosition.X, (int)mousePosition.Y, Color.RayWhite);
    }
}