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
    protected virtual void Move(int[] wantedPosition)
    {
        var wantedChunkPosition = GetChunkPosition(wantedPosition);
        var chunkPosition = GetChunkPosition(Position);
        if (GetTile(wantedPosition, Layer).Blocking || Path == null) return;
        if (chunkPosition.Y != wantedChunkPosition.Y || chunkPosition[1] != wantedChunkPosition[1])
        {
            GetChunk(chunkPosition.Y, chunkPosition[1], Layer).EntityManager.ExitChunk(this);
            GetChunk(wantedChunkPosition.Y, wantedChunkPosition[1], Layer).EntityManager.EnterChunk(this);
            LoadSurroundingChunks(wantedChunkPosition.Y, wantedChunkPosition[1], Layer);
        }
        Position = wantedPosition;
        Path.RemoveAt(0);
    }

    protected virtual void Pathfind(int startY, int startX, int endY, int endX, int layer, int hWeight)
    {
        Path = FindPath(startY, startX, endY, endX, layer, hWeight);
    }

    // TODO: update method to only select tiles that can be seen once vision code is done
    protected virtual void Wander(int hWeight)
    {
        var wanderArea = new List<int[]>();
        var yOffset = Position[0] - _wanderRange;
        var xOffset = Position[1] - _wanderRange;
        
        for (var y = 0; y < 2 * _wanderRange; y++)
        {
            for (var x = 0; x < 2 * _wanderRange; x++)
            {
                var offsetPosition = new Point (x + xOffset, y + yOffset);
                if (GetTile(offsetPosition, 0).Blocking == false)
                {
                    wanderArea.Add(offsetPosition);
                }
            }
        }

        TargetPosition = wanderArea[new Random().Next(0, wanderArea.Count)];
        Pathfind(Position[0], Position[1], TargetPosition[0], TargetPosition[1], 0, hWeight);
    }
}