using System.Collections.Concurrent;
using CaveGame.Generation;
using static CaveGame.GameSettings;
using static CaveGame.Program;

namespace CaveGame.Managers;

public static class ChunkManager
{
    private static ConcurrentDictionary<(int, int, int), Chunk> _loadedChunks = new();
    
    private static ConcurrentDictionary<(int, int, int), Task<Chunk>> _loadingChunks = new();

    public static Chunk GetChunk(int chunkY, int chunkX, int layer)
    {
        var chunkKey = (chunkY, chunkX, layer);
        if (_loadedChunks.TryGetValue(chunkKey, out var loadedChunk))
        {
            return loadedChunk;
        }
        if (_loadingChunks.TryGetValue(chunkKey, out var loadingChunk))
        {
            return loadingChunk.Result;
        }
        var chunkTask = Task.Run(() => LoadChunk(chunkY, chunkX, layer, GetSeed(), _loadedChunks));
        _loadingChunks.TryAdd(chunkKey, chunkTask);
        return chunkTask.Result;
    }

    public static void AddChunk(Chunk chunk)
    {
        _loadedChunks.TryAdd((chunk.Position[1], chunk.Position[0], chunk.Layer), chunk);
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

                var yOffset = y + chunkY;
                var xOffset = x + chunkX;
                var chunkKey = (yOffset, xOffset, layer);
                
                if (_loadedChunks.ContainsKey(chunkKey))
                {
                    continue;
                }
                if (_loadingChunks.ContainsKey(chunkKey))
                {
                    continue;
                }
                var chunkTask = Task.Run(() => LoadChunk(yOffset, xOffset, layer, GetSeed(), _loadedChunks));
                _loadingChunks.TryAdd((yOffset, xOffset, layer), chunkTask);
            }
        }
    }
    
    private static Chunk LoadChunk(int chunkY, int chunkX, int layer, int seed, IDictionary<(int, int, int), Chunk> loadedChunks)
    {
        var chunk = new Chunk(null, new[] { chunkY, chunkX }, layer, new Cave(), seed);
        loadedChunks.TryAdd((chunkY, chunkX, layer), chunk);
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

        var chunk = GetChunk(chunkPosition.Y, chunkPosition.X, layer);
        return chunk.Tiles[tilePosition.Y, tilePosition.X];
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