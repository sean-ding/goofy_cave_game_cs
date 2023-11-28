namespace CaveGame;

public class Tile
{
    public enum States
    {
        Solid,
        Liquid,
        Gas
    }
    
    public string Id = "";
    public ColoredGlyph Glyph = new ();
    public string Name = "";
    public string Description = "";
    protected int MaxHealth;
    public int Health;
    public bool Blocking;
    public States State;
    public List<Item> Items = new List<Item>();
}