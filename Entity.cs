using static CaveGame.Program;
using static CaveGame.GameSettings;
using static CaveGame.Managers.ChunkManager;

namespace CaveGame;

public abstract class Entity
{
    public enum DamageTypes
    {
        Damage,
        Slice,
        Pierce,
        Bludgeon,
        Force,
        Heat,
        Chill,
        Shock,
        Acid,
        Mental,
        Entropic,
        Axiomatic,
        Null
    }
    
    protected abstract string Id { get; }
    public abstract int SpawnWeight { get; }
    protected string Name = "";
    protected string[] Pronouns = new string[3];
    protected readonly int TurnSpeed = TURN_SPEED;
    protected int TurnIndex = 0;
    protected int MaxHealth;

    protected int Health;
    protected int Speed;
    public Point Position;
    public int Layer;
    public SadConsole.Entities.Entity GlyphEntity = new (foreground: Color.Red, background: Color.Black, glyph: 177, zIndex: 0);
    
    public void TakeDamage(int dmg, Enum type, string source)
    {
        Health -= dmg;
        if (Health < 0)
        {
            Die(source);
        }
    }

    private void Die(string source)
    {
        var log = GetGameScreen();
        if (Id == "player")
        {
            switch (source) {
                case "god":
                    log.PrintLog("You flickered out of this plane of existence.", Color.Red);
                    break;
                case "swarmer":
                    log.PrintLog("You were torn apart by a swarmer.", Color.Red);
                    break;
                case "goldenFreddy":
                    log.PrintLog("WAS THAT THE BITE OF 87???", Color.Red);
                    break;
            }
            log.PrintLog("--- YOU DIED ---", Color.Red);
        }
        else
        {
            switch (source) {
                case "god":
                    log.PrintLog(Name + " flickered out of this plane of existence.", Color.Red);
                    break;
                case "swarmer":
                    log.PrintLog(Name + " was torn apart by a swarmer.", Color.White);
                    break;
                case "goldenFreddy":
                    log.PrintLog(Name + " had " + Pronouns[2] + " prefrontal cortex removed.", Color.White);
                    break;
            }
        }
    }

    public Tile getTile()
    {
        Chunk currentChunk = GetChunk(Position.Y, Position.X, Layer); // WTF IS LAYER
        Point positionInChunk = ToLocalPosition(Position);
        Tile tile = currentChunk.Tiles[positionInChunk.Y, positionInChunk.X];
        return tile;
    }
}
