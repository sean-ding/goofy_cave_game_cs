using CaveGame.Generation;
using static CaveGame.GameSettings;
using static CaveGame.Program;

namespace CaveGame.Managers;

public static class ChunkManager
{
    private static Dictionary<(int, int, int), Chunk> _loadedChunks = new();
    
    private static HashSet<(int, int, int)> _loadedChunkRef = new();

    public static Chunk GetChunk(int chunkY, int chunkX, int layer)
        {
        Chunk chunk;
        var chunkKey = (chunkY, chunkX, layer);
        if (_loadedChunkRef.Contains(chunkKey))
        {
            chunk = _loadedChunks[chunkKey];
        }
        else
        {
            chunk = Task.Run(() => LoadChunk(chunkY, chunkX, layer, _loadedChunks)).Result;
        }
        
        return chunk;
    }

    public static void AddChunk(Chunk chunk)
    {
        _loadedChunkRef.Add((chunk.Position[1], chunk.Position[0], chunk.Layer));
        _loadedChunks.Add((chunk.Position[1], chunk.Position[0], chunk.Layer), chunk);
    }

    public static void LoadSurroundingChunks(int chunkY, int chunkX, int layer)
    {
        // northwest
        _ = Task.Run(() => LoadChunk(chunkY - 1, chunkX - 1, layer, _loadedChunks));
        // north
        _ = Task.Run(() => LoadChunk(chunkY - 1, chunkX, layer, _loadedChunks));
        // northeast
        _ = Task.Run(() => LoadChunk(chunkY - 1, chunkX + 1, layer, _loadedChunks));
        // west
        _ = Task.Run(() => LoadChunk(chunkY, chunkX - 1, layer, _loadedChunks));
        // east
        _ = Task.Run(() => LoadChunk(chunkY, chunkX + 1, layer, _loadedChunks));
        // southwest
        _ = Task.Run(() => LoadChunk(chunkY + 1, chunkX - 1, layer, _loadedChunks));
        // south
        _ = Task.Run(() => LoadChunk(chunkY + 1, chunkX, layer, _loadedChunks));
        // southeast
        _ = Task.Run(() => LoadChunk(chunkY + 1, chunkX + 1, layer, _loadedChunks));
    }
    
    private static async Task<Chunk> LoadChunk(int chunkY, int chunkX, int layer, IDictionary<(int, int, int), Chunk> loadedChunks)
    {
        var chunkKey = (chunkY, chunkX, layer);
        if (_loadedChunkRef.Contains(chunkKey))
        {
            return _loadedChunks[chunkKey];
        }
        _loadedChunkRef.Add(chunkKey);
        var createChunkTask = Task.Run(() => new Chunk(null, new[] { chunkY, chunkX }, layer, new Cave(), GetSeed()));
        var chunk = await createChunkTask;
        if (_loadedChunkRef.Contains(chunkKey))
        {
            return _loadedChunks[chunkKey];
        }
        loadedChunks.Add(chunkKey, chunk);
        return chunk;
    }

    public static Point ToLocalPosition(int[] position)
    {
        var chunkY = position[0] % CHUNK_HEIGHT;
        var chunkX = position[1] % CHUNK_WIDTH;

        if (chunkY < 0)
        {
            chunkY += CHUNK_HEIGHT;
        }
        if (chunkX < 0)
        {
            chunkX += CHUNK_WIDTH;
        }

        return new Point(chunkX, chunkY);
    }

    public static Point GetChunkPosition(int[] position)
    {
        int chunkY;
        int chunkX;
        if (position[0] < 0)
        {
            chunkY = (position[0] + 1) / CHUNK_HEIGHT - 1;
        }
        else
        {
            chunkY = position[0] / CHUNK_HEIGHT;
        }
        if (position[1] < 0)
        {
            chunkX = (position[1] + 1) / CHUNK_WIDTH - 1;
        }
        else
        {
            chunkX = position[1] / CHUNK_WIDTH;
        }

        return new Point(chunkX, chunkY);
    }

    public static Tile GetTile(int[] position, int layer)
    {
        var chunkPosition = GetChunkPosition(position);
        var tilePosition = ToLocalPosition(position);

        var tile = GetChunk(chunkPosition.Y, chunkPosition.X, layer).Tiles[tilePosition.Y,tilePosition.X];
        return tile;
    }

    public static Dictionary<(int, int, int), Chunk> GetLoadedChunks()
    {
        Dictionary<(int, int, int), Chunk> loadedChunks = new();
        foreach (var chunk in _loadedChunks)
        {
            loadedChunks.Add(chunk.Key, chunk.Value);
        }

        return loadedChunks;
    }
}