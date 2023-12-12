using static CaveGame.Managers.ChunkManager;

namespace CaveGame.Entities;

public class PlayerEntity : Entity
{
    protected override string Id => "player";
    
    public PlayerEntity(int y, int x, int layer)
    {
        Name = "Player";
        Pronouns = new []{"they", "them", "their"};
        MaxHealth = 100;
        Health = MaxHealth;
        Speed = 10;
        Position = new []{y, x};
        Layer = layer;
        Glyph = new ColoredGlyph(foreground: Color.Blue, background: Color.Black, glyph: '@');
    }
    

}