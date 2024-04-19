using CaveGame.Tiles;
using static CaveGame.Generation.MainGeneration;

namespace CaveGame.Generation;

public class Cave : Biome
{
    public Cave()
    {
        Id = "cave";
        Name = "Cave";
        Description = "Default biome";
    }
    public override Tile[,] GenerateChunk(int height, int width, int chunkY, int chunkX, int seed)
    {
        var walls = GenerateSimplex(height, width, chunkY, chunkX, seed);

        var tileChunk = new Tile[height, width];
        
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (walls[y,x])
                {
                    tileChunk[y, x] = new Stone();
                }
                else
                {
                    tileChunk[y, x] = new Air();
                }
            }
        }
        
        return tileChunk;
    }
}