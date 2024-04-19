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
    public string Name = "";
    protected string[] Pronouns = new string[3];
    protected readonly int TurnSpeed = TURN_SPEED;
    protected int TurnIndex = 0;
    protected int MaxHealth;
    protected int Health;
    protected int Speed;
    public int[] Position = new int[2];
    public int Layer;
    public ColoredGlyph Glyph = new (foreground: Color.Red, background: Color.Black, glyph: 177);
    public List<Item> Inventory = new ();
    
    public void TakeDamage(int dmg, Enum type, string source)
    {
        var log = GetGameScreen();
        Health -= dmg;
        if (Health < 0)
        {
            Die(source);
        }

        if (Id == "player")
        {
            switch (type)
            {
                case DamageTypes.Damage:
                    log.PrintLog("You took " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Slice:
                    log.PrintLog("You were sliced for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Pierce:
                    log.PrintLog("You were pierced for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Bludgeon:
                    log.PrintLog("You were bludgeoned for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Force:
                    log.PrintLog("You were concussed for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Heat:
                    log.PrintLog("You were burned for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Chill:
                    log.PrintLog("You were chilled for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Shock:
                    log.PrintLog("You were shocked for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Acid:
                    log.PrintLog("You were dissolved for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Mental:
                    log.PrintLog("Your mind loosens for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Entropic:
                    log.PrintLog("Your being decays for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Axiomatic:
                    log.PrintLog("You reality is shifted for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Null:
                    log.PrintLog(DateTime.Now + "/> () -> Entity.TakeDamage(player, " + dmg + ");", Color.White);
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case DamageTypes.Damage:
                    log.PrintLog(Name + " took " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Slice:
                    log.PrintLog(Name + " was sliced for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Pierce:
                    log.PrintLog(Name + " was pierced for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Bludgeon:
                    log.PrintLog(Name + " was bludgeoned for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Force:
                    log.PrintLog(Name + " was concussed for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Heat:
                    log.PrintLog(Name + " was burned for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Chill:
                    log.PrintLog(Name + " was chilled for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Shock:
                    log.PrintLog(Name + " was shocked for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Acid:
                    log.PrintLog(Name + " was dissolved for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Mental:
                    log.PrintLog(Name + "'s mind loosens for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Entropic:
                    log.PrintLog(Name + "'s being decays for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Axiomatic:
                    log.PrintLog(Name + "'s reality is shifted for " + dmg + " damage.", Color.Red);
                    break;
                case DamageTypes.Null:
                    log.PrintLog(DateTime.Now + "/> () -> Entity.TakeDamage(" + Id + ", " + dmg + ");", Color.White);
                    break;
            }
        }
    }

    private void Die(string source)
    {
        var log = GetGameScreen();
        if (Id == "player")
        {
            switch (source)
            {
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
            switch (source)
            {
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
}
