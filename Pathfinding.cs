using static CaveGame.Managers.ChunkManager;

namespace CaveGame;

public static class Pathfinding
{
    public static List<Point>? FindPath(int startY, int startX, int endY, int endX, int layer, int hWeight, int openLimit)
    {
        var open = new Dictionary<Point, Node>();
        var closed = new Dictionary<Point, Node>();
        
        var startNode = new Node(startY, startX, hWeight) { G = 0 };
        startNode.SetH(endY, endX);
        
        open.Add(new Point(startX, startY), startNode);

        var pathIndex = 0;
        
        while (open.Count != 0 && pathIndex < openLimit)
        {
            var currentNode = GetCheapest(open);
            var currentKey = new Point(currentNode.X, currentNode.Y);
            open.Remove(currentKey);
            closed.Add(currentKey, currentNode);

            if (currentNode.Y == endY && currentNode.X == endX)
            {
                return CreatePath(currentNode, closed.Count);
            }
            for (var y = -1; y < 2; y++)
            {
                for (var x = -1; x < 2; x++)
                {
                    var childKey = new Point(currentNode.X + x, currentNode.Y + y);
                    
                    if ((y == 0 && x == 0) || closed.ContainsKey(childKey) || GetTile(childKey.Y, childKey.X, layer).Blocking)
                    {
                        continue;
                    }
                    
                    var childNode = new Node(currentNode.Y + y, currentNode.X + x, hWeight, currentNode);
                    childNode.SetH(endY, endX);
                    childNode.SetF();

                    if (open.ContainsKey(childKey))
                    {
                        if (open[childKey].G > childNode.G)
                        {
                            continue;
                        }
                        open.Remove(childKey);
                        open.Add(childKey, childNode);
                    }
                    else
                    {
                        open.Add(childKey, childNode);
                    }
                }
            }

            pathIndex++;
        }

        return null;
    }

    private static Node GetCheapest(Dictionary<Point, Node> open)
    {
        Node? cheapest = null;
        foreach (var node in open)
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

    private static List<Point>? CreatePath(Node endNode, int length)
    {
        var node = endNode;
        var path = new List<Point>();
        for (var i = 0; i < length; i++)
        {
            path.Insert(0, new Point(node.X, node.Y));
            if (node.Parent == null)
            {
                return path;
            }
            node = node.Parent;
        }

        return null;
    }
    
    private class Node
    {
        public readonly int Y, X;
        public int F, G, H;
        private readonly int _hWeight;
        public readonly Node? Parent;

        public Node(int y, int x, int hWeight, Node? parent = null)
        {
            Y = y;
            X = x;
            _hWeight = hWeight;
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
            F = G + _hWeight * H;
        }

        public void SetH(int y, int x)
        {
            var dY = Y - y;
            var dX = X - x;
            H = dY * dY + dX * dX;
        }
    }
}