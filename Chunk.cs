using CaveGame.Managers;
using SadConsole.Entities;
using static CaveGame.GameSettings;
using static CaveGame.Managers.BiomeManager;
using static CaveGame.Managers.TileManager;
using EntityManager = CaveGame.Managers.EntityManager;

namespace CaveGame;

public class Chunk
{
    public Tile[,] Tiles;
    public bool[,] Blocking;
    public readonly int Width;
    public readonly int Height;
    public readonly int[] Position;
    public int Layer;
    public EntityManager EntityManager;
    public readonly int Seed;
    public Biome Biome;

    public Chunk(Tile[,]? tiles, int[] position, int layer, Biome biome, int seed, int height = CHUNK_HEIGHT, int width = CHUNK_WIDTH)
    {
        Height = height;
        Width = width;
        Tiles = new Tile[Height, Width];
        Position = position;
        Layer = layer;
        EntityManager = new EntityManager();
        Seed = seed;
        Biome = biome;
        if (tiles == null)
        {
            Tiles = biome.GenerateChunk(Height, Width, Position[0], Position[1], Seed);
        }
        else
        {
            Tiles = tiles;
        }

        Blocking = new bool[Height, Width];
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                Blocking[y,x] = Tiles[y,x].Blocking;
            }
        }
    }
}