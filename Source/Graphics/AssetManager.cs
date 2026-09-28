using Raylib_cs;
using System.Xml.Linq;

using Overtile.Source.World;
using Overtile.Source.Entities;

namespace Overtile.Source.Graphics;

public class AssetManager
{
    private readonly Dictionary<TileType, Texture2D> _tilesTextures = new();
    // private readonly Dictionary<PlayerState, Texture2D> _playerSkins = new();
    private readonly Dictionary<string, Rectangle> _playerStates = new();
    public Texture2D[] PlayerTexture = new Texture2D[27];

    public void LoadPlayerContent()
    {
        PlayerTexture[0] = Raylib.LoadTexture("./Assets/char_a_p1_0bas_humn_v00.png");
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
    public void LoadMapContent()
    {
        // Map Tiles
        _tilesTextures[TileType.Grass] = Raylib.LoadTexture("./Assets/tile_0000.png");
        _tilesTextures[TileType.StoneWallBorder] = Raylib.LoadTexture("./Assets/tile_0109.png");
        
        // Player State
        // _playerSkins[PlayerState.Default] = Raylib.LoadTexture("./Assets/character_maleAdventurer_side.png");
        
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

    public Rectangle GetPlayerRec(string textureName)
    {
        if (_playerStates.TryGetValue(textureName, out Rectangle rectangle))
        {
            return rectangle;
        }

        throw new KeyNotFoundException($"No texture for {textureName}");
    }

    public void UnloadMapContent()
    {
        foreach (var texture in _tilesTextures.Values)
        {
            Raylib.UnloadTexture(texture);
        }
        _tilesTextures.Clear();
    }

    public void UnloadPlayerContent()
    {
        foreach (var i in PlayerTexture)
        {
            Raylib.UnloadTexture(i);
        }

        _playerStates.Clear();
    }
}