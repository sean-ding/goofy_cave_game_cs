using static SadRogue.Primitives.Color;

namespace CaveGame.Tiles;

public class Stone : Tile
{
    public Stone()
    {
        Id = "stone";
        Glyph = new ColoredGlyph(White, Transparent, '#');
        MaxHealth = 1000;
        Health = MaxHealth;
        Blocking = true;
        Transparency = 0;
        State = States.Solid;
    }
}