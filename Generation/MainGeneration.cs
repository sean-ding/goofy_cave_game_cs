using CaveGame.Managers;
using static CaveGame.Managers.BiomeManager;
using static CaveGame.GameSettings;

namespace CaveGame.Generation;

public static class MainGeneration
{
    public static bool[,] GenerateSimplex(int height, int width, int chunkY, int chunkX, int seed)
    {
        var yOffset = chunkY * height;
        var xOffset = chunkX * width;
        
        var walls = new bool[height,width];
        
        SimplexNoise.Seed = seed;
        var noiseGrid = SimplexNoise.Calc2D(width, height, xOffset, yOffset, 0.05f);
        
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (noiseGrid[y,x] > SIMPLEX_WALL_THRESHOLD)
                {
                    walls[y,x] = false;
                }
                else
                {
                    walls[y,x] = true;
                }
            }
        }

        System.Console.WriteLine("Region Generated: " + chunkX + ", " + chunkY);
        System.Console.WriteLine("Seed: " + seed);
        
        ThreadPool.GetMaxThreads(out var maxThreads, out _);

        ThreadPool.GetAvailableThreads(out int availableThreads, out _);

        System.Console.WriteLine("Running Threads: " + (maxThreads - availableThreads));
        return walls;
    }
    
    // bad at method names
    // example use: find large enough area to spawn player/staircase
    public static int[,]? FindAreaThatIsOfProvidedArea(Chunk chunk, int area)
    {
        var walls = chunk.Blocking;
        var height = chunk.Height;
        var width = chunk.Width;
        var startPoints = new List<int[]>();
        
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (!walls[y,x])
                {
                    startPoints.Add(new []{y,x});
                }
            }
        }

        while (startPoints.Count > 0)
        {
            var startPointIndex = SHutil.Random(0, startPoints.Count);
            
            var visited = new bool[height, width];
            
            var areaCheckResult = AreaCheck(startPoints[startPointIndex][0], startPoints[startPointIndex][1], visited, walls, area);
            _visitedArea = 0;

            if (areaCheckResult)
            {
                var finalAreaPoints = new int[area, 2];
                var pointIndex = 0;

                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        if (_visitedResult[y, x])
                        {
                            finalAreaPoints[pointIndex, 0] = y;
                            finalAreaPoints[pointIndex, 1] = x;
                            pointIndex++;
                        }
                    }
                }

                return finalAreaPoints;
            }
            
            // iterate over startPoints list backwards to allow for removal
            for (int i = startPoints.Count - 1; i >= 0; i--)
            {
                if (_visitedResult[startPoints[i][0], startPoints[i][1]]) { startPoints.RemoveAt(i); }
            }
            _visitedResult = null;
        }

        return null;
    }
    
    private static int _visitedArea;
    private static bool[,]? _visitedResult;
    
    private static bool AreaCheck(int y, int x, bool[,] visited, bool[,] walls, int area) 
    {
        if (_visitedArea >= area) { return true; }
        if (!(0 <= y && y <= walls.GetLength(0) - 1 && 0 <= x && x <= walls.GetLength(1) - 1)) { return false; }
        if (visited[y,x] || walls[y,x]) { return false; }
        _visitedArea++;
        visited[y,x] = true;
        if (AreaCheck(y + 1, x, visited, walls, area) || AreaCheck(y-1, x, visited, walls, area) || AreaCheck(y, x+1, visited, walls, area) || AreaCheck(y, x-1, visited, walls, area))
        {
            if (_visitedResult == null) { _visitedResult = visited; }
            return true;
        }
        if (_visitedResult == null) { _visitedResult = visited; }
        return false;
    }
}