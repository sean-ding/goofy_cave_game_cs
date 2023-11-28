using CaveGame.Generation;
using static CaveGame.GameSettings;
using static CaveGame.Program;

namespace CaveGame.Managers;

public static class ChunkManager
{
    private static Dictionary<(int, int, int), Chunk> _loadedChunks = new();
    
    private static Dictionary<(int, int, int), Task<Chunk>> _loadingChunks = new();

    public static Chunk GetChunk(int chunkY, int chunkX, int layer)
    {
        var chunkKey = (chunkY, chunkX, layer);
        if (_loadingChunks.TryGetValue(chunkKey, out var loadingChunk))
        {
            var chunkTask = Task.Run(async () => await loadingChunk);
            return chunkTask.Result;
        }
        if (_loadedChunks.TryGetValue(chunkKey, out var loadedChunk))
        {
            return loadedChunk;
        }
        return LoadChunk(chunkY, chunkX, layer);
    }

    public static void AddChunk(Chunk chunk)
    {
        _loadedChunks.Add((chunk.Position[1], chunk.Position[0], chunk.Layer), chunk);
    }

    public static void LoadSurroundingChunks(int chunkY, int chunkX, int layer)
    {
        for (var y = -1; y < 2; y++)
        {
            for (var x = -1; x < 2; x++)
            {
                if (y == 0 && x == 0)
                {
                    continue;
                }

                LoadChunk(chunkY + y, chunkX + x, layer);
            }
        }
    }

    private static Chunk LoadChunk(int chunkY, int chunkX, int layer)
    {
        Task<Chunk> chunkTask;
        _loadingChunks.Add((chunkY, chunkX, layer), chunkTask);
        return chunkTask.Result;
    }
    
    private static async Task<Chunk> LoadChunkAsync(int chunkY, int chunkX, int layer, IDictionary<(int, int, int), Chunk> loadedChunks)
    {
        var chunkKey = (chunkY, chunkX, layer);
        if (_loadingChunks.TryGetValue(chunkKey, out var loadingChunk))
        {
            await loadingChunk;
            return _loadedChunks[chunkKey];
        }
        if (_loadedChunks.TryGetValue(chunkKey, out var loadedChunk))
        {
            return loadedChunk;
        }
        var createChunkTask = Task.Run(() => new Chunk(null, new[] { chunkY, chunkX }, layer, new Cave(), GetSeed()));
        var chunk = await createChunkTask;
        loadedChunks.Add(chunkKey, chunk);
        return chunk;
    }

    public static Point ToLocalPosition(int positionY, int positionX)
    {
        var chunkY = positionY % CHUNK_HEIGHT;
        var chunkX = positionX % CHUNK_WIDTH;

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

    public static Point GetChunkPosition(int positionY, int positionX)
    {
        int chunkY;
        int chunkX;
        if (positionY < 0)
        {
            chunkY = (positionY + 1) / CHUNK_HEIGHT - 1;
        }
        else
        {
            chunkY = positionY / CHUNK_HEIGHT;
        }
        if (positionX < 0)
        {
            chunkX = (positionX + 1) / CHUNK_WIDTH - 1;
        }
        else
        {
            chunkX = positionX / CHUNK_WIDTH;
        }

        return new Point(chunkX, chunkY);
    }

    public static Tile GetTile(int positionY, int positionX, int layer)
    {
        var chunkPosition = GetChunkPosition(positionY, positionX);
        var tilePosition = ToLocalPosition(positionY, positionX);

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