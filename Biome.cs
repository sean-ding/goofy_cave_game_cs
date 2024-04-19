namespace CaveGame;

public abstract class Biome
{
    public string Id = "";
    public string Name = "";
    public string Description = "";
    public abstract Tile[,] GenerateChunk(int height, int width, int chunkY, int chunkX, int seed);
}