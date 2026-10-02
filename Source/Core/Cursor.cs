using System.Numerics;
using Overtile.Source.Graphics;
using Overtile.Source.World;
using Raylib_cs;

namespace Overtile.Source.Core;

public class Cursor
{
    private AssetManager AssetManager { get; }
    private Texture2D CursorTexture { get; }
    
    public Cursor(AssetManager assetManager)
    {
        AssetManager = assetManager;
        CursorTexture = AssetManager.GetMiscTexture(MiscTextureType.Cursor);
        
        Raylib.HideCursor();
    }

    public void UpdateCursor()
    {
        Vector2 mousePosition = Raylib.GetMousePosition();
        
        Raylib.DrawTexture(CursorTexture, (int)mousePosition.X, (int)mousePosition.Y, Color.RayWhite);
    }
}