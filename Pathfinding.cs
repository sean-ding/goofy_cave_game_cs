using static CaveGame.Managers.ChunkManager;

namespace CaveGame;

public class Pathfinding
{
    private Dictionary<Point, Node> Open;

    private Dictionary<Point, Node> Closed;
    
    private Node StartNode;
    private Node EndNode;

    public int[,]? FindPath(int startY, int startX, int endY, int endX, int layer, int hWeight)
    {
        StartNode = new Node(startY, startX, hWeight) { G = 0 };
        StartNode.SetH(endY, endX);
        
        Open.Add(new Point(startX, startY), StartNode);

        while (Open.Count != 0)
        {
            var currentNode = GetCheapest();
            Open.Remove(new Point(currentNode.X, currentNode.Y));
            Closed.Add(new Point(currentNode.X, currentNode.Y), currentNode);

            if (currentNode.Y == endY && currentNode.X == endX)
            {
                return null; // make this return path
            }
            for (var y = 0; y < 2; y++)
            {
                for (var x = 0; x < 2; x++)
                {
                    var childKey = new Point(currentNode.X + x, currentNode.Y + y);
                    var chunkPosition = GetChunkPosition(childKey);
                    var chunk = GetChunk(chunkPosition.Y, chunkPosition.X, layer);
                    
                    if ((y == 0 && x == 0) || Closed.ContainsKey(childKey) || chunk.Blocking[childKey.Y, childKey.X])
                    {
                        continue;
                    }
                    
                    var childNode = new Node(currentNode.Y + y, currentNode.X + x, hWeight, currentNode);
                    childNode.SetH(endY, endX);
                    childNode.SetF();

                    if (Open.ContainsKey(childKey))
                    {
                        if (Open[childKey].G > childNode.G)
                        {
                            continue;
                        }
                        Open.Remove(childKey);
                        Open.Add(childKey, childNode);
                    }
                    else
                    {
                        Open.Add(childKey, childNode);
                    }
                }
            }
        }

        return null;
    }

    private Node GetCheapest()
    {
        Node? cheapest = null;
        foreach (var node in Open)
        {
            if (cheapest == null)
            {
                cheapest = node.Value;
                continue;
            }
            if (cheapest.F > node.Value.F)
            {
                cheapest = node.Value;
            }

            if (cheapest.F == node.Value.F)
            {
                if (cheapest.H > node.Value.H)
                {
                    cheapest = node.Value;
                }
            }
        }

        return cheapest!;
    }
    
    private class Node
    {
        public int Y, X;
        public int F, G, H;
        public int HWeight;
        public Node? Parent;

        public Node(int y, int x, int hWeight, Node? parent = null)
        {
            Y = y;
            X = x;
            HWeight = hWeight;
            Parent = parent;
            if (Parent == null)
            {
                G = 0;
            }
            else
            {
                G = Parent.G + (int) (10.0 * Math.Sqrt(Math.Pow(Parent.Y - Y, 2) + Math.Pow(Parent.X - X, 2)));
            }
        }
        
        public void SetF()
        {
            F = G + HWeight * H;
        }

        public void SetH(int y, int x)
        {
            var dY = Y - y;
            var dX = X - x;
            H = dY * dY + dX * dX;
        }
    }
}