using Raylib_cs;

namespace GameProject.Source;

public class AssetManager
{
    private readonly Dictionary<ETileType, Texture2D> _tilesTextures = new();

    public void LoadContent()
    {
        _tilesTextures[ETileType.Grass] = Raylib.LoadTexture("./Assets/tile_0000.png");
        _tilesTextures[ETileType.StoneWallBorder] = Raylib.LoadTexture("./Assets/tile_0109.png");
    }

    public Texture2D GetTexture(ETileType type)
    {
        if (_tilesTextures.TryGetValue(type, out Texture2D texture))
        {
            return texture;
        }

        throw new KeyNotFoundException($"No texture for {type}");
    }

    public void UnloadContent()
    {
        foreach (var texture in _tilesTextures.Values)
        {
            Raylib.UnloadTexture(texture);
        }
        _tilesTextures.Clear();
    }
}