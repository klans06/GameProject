using Raylib_cs;
using System.Xml.Linq;

using Overtile.Source.World;
using Overtile.Source.Entities;

namespace Overtile.Source.Graphics;

public class AssetManager
{
    private Dictionary<TileType, Texture2D> _tilesTextures = new();
    // private readonly Dictionary<PlayerState, Texture2D> _playerSkins = new();
    private readonly Dictionary<string, Rectangle> _playerStates = new();
    public Texture2D PlayerTexture;

    /// <summary>
    /// Loads the player sprite sheet and reads named frame rectangles from its XML atlas.
    /// </summary>
    public void LoadPlayerContent()
    {
        PlayerTexture = Raylib.LoadTexture("./Assets/char_a_p1_0bas_humn_v00.png");
        string xmlPath = "./Assets/Atlases/player_walk.xml";

        XDocument doc = XDocument.Load(xmlPath);
        foreach (var element in doc.Root.Elements("SubTexture"))
        {
            string name = element.Attribute("name")?.Value;
            float x = float.Parse(element.Attribute("x")?.Value ?? "0");
            float y = float.Parse(element.Attribute("y")?.Value ?? "0");
            float width = float.Parse(element.Attribute("width")?.Value ?? "64");
            float height = float.Parse(element.Attribute("height")?.Value ?? "64");
            
            if (name != null)
            {
                _playerStates[name] = new Rectangle(x, y, width, height);
            }
        }

    }
    /// <summary>
    /// Loads the grass, stone wall border, and decorative prop textures used to render the map.
    /// </summary>
    public void LoadMapContent()
    {
        // Map Tiles
        _tilesTextures[TileType.Grass] = Raylib.LoadTexture("./Assets/Map/tile_0000.png");
        _tilesTextures[TileType.GrassWeed] = Raylib.LoadTexture("./Assets/Map/tile_0001.png");
        _tilesTextures[TileType.GrassWFlowers] = Raylib.LoadTexture("./Assets/Map/tile_0002.png");
        _tilesTextures[TileType.StoneWallBorder] = Raylib.LoadTexture("./Assets/Map/tile_0109.png");
        
        // Map Prop Tiles
        _tilesTextures[TileType.PropShrub] = Raylib.LoadTexture("./Assets/Map/tile_0005.png");
        _tilesTextures[TileType.PropLongGrass] = Raylib.LoadTexture("./Assets/Map/tile_0017.png");
        _tilesTextures[TileType.PropMushrooms] = Raylib.LoadTexture("./Assets/Map/tile_0029.png");
        
        // Building Tiles
    }

    /// <summary>
    /// Retrieves the loaded texture for a map tile type.
    /// </summary>
    /// <param name="type">The tile type whose texture is requested.</param>
    /// <returns>The loaded tile texture.</returns>
    /// <exception cref="KeyNotFoundException">No texture is loaded for the requested tile type.</exception>
    public Texture2D GetMapTexture(TileType type)
    {
        if (_tilesTextures.TryGetValue(type, out Texture2D texture))
        {
            return texture;
        }

        throw new KeyNotFoundException($"No texture for {type}");
    }

    /// <summary>
    /// Retrieves a named frame rectangle from the loaded player sprite atlas.
    /// </summary>
    /// <param name="textureName">The exact frame name defined in the atlas.</param>
    /// <returns>The frame's source rectangle within the player sprite sheet.</returns>
    /// <exception cref="KeyNotFoundException">The requested frame name is not loaded.</exception>
    public Rectangle GetPlayerRec(string textureName)
    {
        if (_playerStates.TryGetValue(textureName, out Rectangle rectangle))
        {
            return rectangle;
        }

        throw new KeyNotFoundException($"No texture for {textureName}");
    }

    /// <summary>
    /// Unloads all map tile textures and clears their lookup table.
    /// </summary>
    public void UnloadMapContent()
    {
        foreach (var texture in _tilesTextures.Values)
        {
            Raylib.UnloadTexture(texture);
        }
        _tilesTextures.Clear();
    }

    /// <summary>
    /// Unloads the player sprite sheet and clears the cached atlas frame rectangles.
    /// </summary>
    public void UnloadPlayerContent()
    {
        Raylib.UnloadTexture(PlayerTexture);
        _playerStates.Clear();
    }
}