using CaveGame.Generation;
using static CaveGame.GameSettings;
using static CaveGame.Program;

namespace CaveGame.Managers;

public static class ChunkManager
{
    private static List<Chunk> _loadedChunks = new();

    public static Chunk GetChunk(int chunkY, int chunkX, int layer)
    {
        var chunk = _loadedChunks.Find(loadedChunk => loadedChunk.Position[0] == chunkY && loadedChunk.Position[1] == chunkX);

        if (chunk == null)
        {
            chunk = Task.Run(() => LoadChunk(chunkY, chunkX, layer)).Result;
        }

        return chunk;
    }

    public static void AddChunk(Chunk chunk)
    {
        _loadedChunks.Add(chunk);
    }

    public static void LoadSurroundingChunks(int chunkY, int chunkX, int layer)
    {
        // northwest
        _ = Task.Run(() => LoadChunk(chunkY - 1, chunkX - 1, layer));
        // north
        _ = Task.Run(() => LoadChunk(chunkY - 1, chunkX, layer));
        // northeast
        _ = Task.Run(() => LoadChunk(chunkY - 1, chunkX + 1, layer));
        // west
        _ = Task.Run(() => LoadChunk(chunkY, chunkX - 1, layer));
        // east
        _ = Task.Run(() => LoadChunk(chunkY, chunkX + 1, layer));
        // southwest
        _ = Task.Run(() => LoadChunk(chunkY + 1, chunkX - 1, layer));
        // south
        _ = Task.Run(() => LoadChunk(chunkY + 1, chunkX, layer));
        // southeast
        _ = Task.Run(() => LoadChunk(chunkY + 1, chunkX + 1, layer));
    }
    
    public static async Task<Chunk> LoadChunk(int chunkY, int chunkX, int layer)
    {
        var foundChunk = _loadedChunks.Find(loadedChunk => loadedChunk.Position[0] == chunkY && loadedChunk.Position[1] == chunkX);
        if (foundChunk != null) { return foundChunk; }
        var createChunkTask = Task.Run(() => new Chunk(null, new[] { chunkY, chunkX }, layer, new Cave(), GetSeed()));
        var chunk = await createChunkTask;
        foundChunk = _loadedChunks.Find(loadedChunk => loadedChunk.Position[0] == chunkY && loadedChunk.Position[1] == chunkX);
        if (foundChunk != null) { return foundChunk; }
        _loadedChunks.Add(chunk);
        return chunk;
    }

    public static Point ToLocalPosition(Point position)
    {
        var chunkPositionY = position.Y % CHUNK_HEIGHT;
        var chunkPositionX = position.X % CHUNK_WIDTH;

        if (chunkPositionY < 0)
        {
            chunkPositionY += CHUNK_HEIGHT;
        }
        if (chunkPositionX < 0)
        {
            chunkPositionX += CHUNK_WIDTH;
        }
        
        return new Point(chunkPositionY, chunkPositionX);
    }

    public static Point GetChunkPosition(Point position)
    {
        int chunkY;
        int chunkX;
        if (position.Y < 0)
        {
            chunkY = (position.Y + 1) / CHUNK_HEIGHT - 1;
        }
        else
        {
            chunkY = position.Y / CHUNK_HEIGHT;
        }
        if (position.X < 0)
        {
            chunkX = (position.X + 1) / CHUNK_WIDTH - 1;
        }
        else
        {
            chunkX = position.X / CHUNK_WIDTH;
        }
        
        System.Console.WriteLine("Chunk Position: " + chunkX + ", " + chunkY);
        return new Point(chunkY, chunkX);
    }
}