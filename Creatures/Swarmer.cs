namespace CaveGame.Creatures;

public class Swarmer : NonPlayerEntity
{
    protected override string Id => "swarmer";
    public override int SpawnWeight => 10;

    public Swarmer()
    {
        Name = "Swarmer";
        Pronouns = new []{"it", "it", "itself"};
        MaxHealth = 10;
        Health = MaxHealth;
        Speed = 10;
        GlyphEntity = new SadConsole.Entities.Entity(foreground: Color.Red, background: Color.Black, glyph: '*', zIndex: 0);
    }

    public override void Turn()
    {
        
    }

    protected override void Move(int[] wantedPosition)
    {
        
    }
}