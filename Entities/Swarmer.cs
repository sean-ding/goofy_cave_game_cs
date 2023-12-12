namespace CaveGame.Entities;

public class Swarmer : Creature
{
    protected override string Id => "swarmer";
    public override int SpawnWeight => 10;

    public Swarmer(int y, int x, int layer)
    {
        Name = "Swarmer";
        Pronouns = new[] { "it", "it", "itself" };
        MaxHealth = 10;
        Health = MaxHealth;
        Speed = 10;
        Position = new[] { y, x };
        Layer = layer;
        Glyph = new ColoredGlyph(foreground: Color.Red, background: Color.Black, glyph: '*');
    }

    public override void Turn()
    {
        TurnIndex += Speed;

        while (TurnIndex >= TurnSpeed)
        {
            TurnIndex -= TurnSpeed;

            if (Path == null)
            {
                Wander(5, 200);
            }
            else if (Position[0] == TargetPosition[0] && Position[1] == TargetPosition[1])
            {
                Wander(5, 200);
            }
            
            if (Path != null && Path.Count > 0)
            {
                Move(Path[0]);
            }
        }
    }
}