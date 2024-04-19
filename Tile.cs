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
    protected int MaxHealth;
    public int Health;
    public bool Blocking;
    public double Transparency;
    public States State;
    public List<Item> Items = new List<Item>();
}