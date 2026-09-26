using Raylib_cs;

namespace GameProject.Source;

public class AssetManager
{
    private readonly Dictionary<TileType, Texture2D> _tilesTextures = new();
    private readonly Dictionary<PlayerState, Texture2D> _playerSkins = new();

    public void LoadContent()
    {
        // Map Tiles
        _tilesTextures[TileType.Grass] = Raylib.LoadTexture("./Assets/tile_0000.png");
        _tilesTextures[TileType.StoneWallBorder] = Raylib.LoadTexture("./Assets/tile_0109.png");
        
        // Player State
        _playerSkins[PlayerState.Default] = Raylib.LoadTexture("./Assets/character_maleAdventurer_side.png");
        
        // Building Tiles
        
    }

    public Texture2D GetMapTexture(TileType type)
    {
        if (_tilesTextures.TryGetValue(type, out Texture2D texture))
        {
            return texture;
        }

        throw new KeyNotFoundException($"No texture for {type}");
    }

    public Texture2D GetPlayerTexture(PlayerState type)
    {
        if (_playerSkins.TryGetValue(type, out Texture2D texture))
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