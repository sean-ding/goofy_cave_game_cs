using static CaveGame.Managers.ChunkManager;
using static CaveGame.Pathfinding;

namespace CaveGame;

public abstract class Creature : Entity
{
    public abstract int SpawnWeight { get; }
    protected int[] TargetPosition = new int[2];
    protected List<Point>? Path;
    private int _wanderRange = 10;
    
    public abstract void Turn();
    protected virtual void Move(Point wantedPosition)
    {
        var wantedChunkPosition = GetChunkPosition(wantedPosition.Y, wantedPosition.X);
        var chunkPosition = GetChunkPosition(Position[0], Position[1]);
        
        if (GetTile(wantedPosition.Y, wantedPosition.X, Layer).Blocking || Path == null) return;
        
        if (chunkPosition.Y != wantedChunkPosition.Y || chunkPosition.X != wantedChunkPosition.X)
        {
            GetChunk(chunkPosition.Y, chunkPosition.X, Layer).EntityManager.ExitChunk(this);
            GetChunk(wantedChunkPosition.Y, wantedChunkPosition.X, Layer).EntityManager.EnterChunk(this);
            LoadSurroundingChunks(wantedChunkPosition.Y, wantedChunkPosition.X, Layer);
        }
        Position[0] = wantedPosition.Y;
        Position[1] = wantedPosition.X;
        Path.RemoveAt(0);
    }

    protected virtual void Pathfind(int startY, int startX, int endY, int endX, int layer, int hWeight, int openLimit)
    {
        Path = FindPath(startY, startX, endY, endX, layer, hWeight, openLimit);
    }

    // TODO: update method to only select tiles that can be seen once vision code is done
    protected virtual void Wander(int hWeight, int openLimit)
    {
        var wanderArea = new List<Point>();
        var yOffset = Position[0] - _wanderRange;
        var xOffset = Position[1] - _wanderRange;
        
        for (var y = 0; y < 2 * _wanderRange; y++)
        {
            for (var x = 0; x < 2 * _wanderRange; x++)
            {
                var offsetPosition = new Point (x + xOffset, y + yOffset);
                if (GetTile(offsetPosition.Y, offsetPosition.X, 0).Blocking == false)
                {
                    wanderArea.Add(offsetPosition);
                }
            }
        }

        var targetPoint = wanderArea[new Random().Next(0, wanderArea.Count)];
        TargetPosition[0] = targetPoint.Y;
        TargetPosition[1] = targetPoint.X;
        Pathfind(Position[0], Position[1], TargetPosition[0], TargetPosition[1], 0, hWeight, openLimit);
    }
}