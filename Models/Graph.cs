using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.ObjectPool;
using Microsoft.VisualBasic;

namespace veriYapilari.Models;


public class GraphNode
{
    public string Id;
    public List<GraphEdge> Neighbors;

    public GraphNode(string id)
    {
        Id = id;
        Neighbors = new List<GraphEdge>();
    }
}

public class GraphEdge
{
    public GraphNode TargetNode;
    public int Weight;

    public GraphEdge(GraphNode targetNode, int weight)
    {
        TargetNode = targetNode;
        Weight = weight;
    }
}

public class CustomGraph
{
    private Dictionary<string, GraphNode> nodes = new();

    public void AddNode(string id)
    {
        if (!nodes.ContainsKey(id))
            nodes[id] = new GraphNode(id);
    }

    public void AddEdge(string sourceId, string targetId, int weight)
    {
        var source = nodes[sourceId];
        var target = nodes[targetId];
        source.Neighbors.Add(new GraphEdge(target, weight));
    }

    public GraphNode? GetNode(string id)
    {
        return nodes.ContainsKey(id) ? nodes[id] : null;
    }

    public void PrintGraph()  // Komşuluk listesini yazdırma metodu
    {
        foreach (var node in nodes)
        {
            Console.WriteLine($"Node {node.Key}:");

            foreach (var edge in node.Value.Neighbors)
            {
                Console.WriteLine($"  -> {edge.TargetNode.Id} (Weight: {edge.Weight})");
            }
            Console.WriteLine();
        }
    }
    public Dictionary<string, int> Dijkstra(string startId)
{
    var distances = new Dictionary<string, int>();
    var visited = new HashSet<string>();
    var priorityQueue = new SortedSet<(int distance, string nodeId)>();

    // Tüm düğümler için başlangıçta mesafeleri sonsuz olarak ayarla
    foreach (var node in nodes.Keys)
    {
        distances[node] = int.MaxValue;
    }

    // Başlangıç düğümünün mesafesini sıfır yap
    distances[startId] = 0;
    priorityQueue.Add((0, startId));

    while (priorityQueue.Count > 0)
    {
        var (currentDistance, currentId) = priorityQueue.Min;
        priorityQueue.Remove(priorityQueue.Min);

        if (visited.Contains(currentId))
            continue;

        visited.Add(currentId);
        var currentNode = nodes[currentId];

        foreach (var edge in currentNode.Neighbors)
        {
            var neighbor = edge.TargetNode;
            int weight = edge.Weight;

            if (visited.Contains(neighbor.Id))
                continue;

            int newDist = currentDistance + weight;

            if (newDist < distances[neighbor.Id])
            {
                // Eski giriş varsa kaldır, çünkü SortedSet duplicate istemez
                priorityQueue.Remove((distances[neighbor.Id], neighbor.Id));
                distances[neighbor.Id] = newDist;
                priorityQueue.Add((newDist, neighbor.Id));
            }
        }
    }

    return distances;
}

    
}
