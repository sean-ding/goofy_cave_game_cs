using static CaveGame.Managers.ChunkManager;
using static CaveGame.Pathfinding;
using static CaveGame.Viewcast;

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
    protected virtual void Wander(int maxTries, int hWeight, int openLimit)
    {
        var wanderArea = GetShadowMask(Position[0], Position[1], Layer, _wanderRange, _wanderRange);
        var rand = new Random();

        for (var i = 0; i < maxTries; i++)
        {
            var guessY = rand.Next(0, _wanderRange * 2 + 1);
            var guessX = rand.Next(0, _wanderRange * 2 + 1);
            if (wanderArea[guessY, guessX])
            {
                TargetPosition[0] = Position[0] + (guessY - _wanderRange / 2);
                TargetPosition[1] = Position[1] + (guessX - _wanderRange / 2);
                Pathfind(Position[0], Position[1], TargetPosition[0], TargetPosition[1], 0, hWeight, openLimit);
                return;
            }
        }
    }
}