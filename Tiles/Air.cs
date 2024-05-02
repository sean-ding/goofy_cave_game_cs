using static SadRogue.Primitives.Color;

namespace CaveGame.Tiles;

public class Air : Tile
{
    public Air()
    {
        Id = "air";
        Glyph = new ColoredGlyph(DarkGray, Transparent, 250);
        MaxHealth = -1;
        Health = MaxHealth;
        Blocking = false;
        Transparency = 0;
        State = States.Gas;
    }
}