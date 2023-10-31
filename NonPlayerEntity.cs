namespace CaveGame;

public abstract class NonPlayerEntity : Entity
{
    public int[] TargetPosition = new int[2];

    public virtual void Spawn(int y, int x, int layer)
    {
        Position = new Point(y, x);
        Layer = layer;
    }
    public abstract void Turn();
    protected virtual void Move(int[] wantedPosition)
    {
        
    }
}