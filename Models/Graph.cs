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
    public List<(string from, string to, int weight)> PrimMST(string startNodeId)  // Prim algoritması
    {
        var mstEdges = new List<(string from, string to, int weight)>();
        var visited = new HashSet<string>();
        var priorityQueue = new PriorityQueue<(GraphNode from, GraphEdge edge), int>();

        var startNode = GetNode(startNodeId);
        if (startNode == null)
            throw new ArgumentException("Start node not found.");

        visited.Add(startNode.Id);

        // Başlangıç düğümünden çıkan tüm kenarları kuyruğa ekle
        foreach (var edge in startNode.Neighbors)
        {
            priorityQueue.Enqueue((startNode, edge), edge.Weight);
        }

        while (priorityQueue.Count > 0)
        {
            var (fromNode, edge) = priorityQueue.Dequeue();
            var toNode = edge.TargetNode;

            if (visited.Contains(toNode.Id))
                continue;

            // MST'ye bu kenarı ekle
            mstEdges.Add((fromNode.Id, toNode.Id, edge.Weight));
            visited.Add(toNode.Id);

            // Yeni düğümün komşularını kuyruğa ekle
            foreach (var nextEdge in toNode.Neighbors)
            {
                if (!visited.Contains(nextEdge.TargetNode.Id))
                {
                    priorityQueue.Enqueue((toNode, nextEdge), nextEdge.Weight);
                }
            }
        }

        return mstEdges;
    }
    public List<(string SourceId, GraphEdge Edge)> Kruskal()
{
    var mst = new List<(string SourceId, GraphEdge Edge)>();
    var edgeList = new List<(string SourceId, GraphEdge Edge)>();
    var added = new HashSet<string>();  // Çift yönlü tekrarları önlemek için

    // Kenarları toplarken source bilgisini de ekliyoruz
    foreach (var node in nodes)
    {
        foreach (var edge in node.Value.Neighbors)
        {
            string key = node.Key + "-" + edge.TargetNode.Id;
            string reverseKey = edge.TargetNode.Id + "-" + node.Key;
            if (!added.Contains(key) && !added.Contains(reverseKey))
            {
                edgeList.Add((node.Key, edge));
                added.Add(key);
            }
        }
    }

    // Ağırlıklarına göre sıralıyoruz
    edgeList.Sort((a, b) => a.Edge.Weight.CompareTo(b.Edge.Weight));

    var disjointSet = new Dictionary<string, string>();
    foreach (var node in nodes.Keys)
    {
        disjointSet[node] = node;
    }

    string Find(string node)
    {
        if (disjointSet[node] != node)
            disjointSet[node] = Find(disjointSet[node]);
        return disjointSet[node];
    }

    void Union(string node1, string node2)
    {
        string root1 = Find(node1);
        string root2 = Find(node2);
        if (root1 != root2)
            disjointSet[root2] = root1;
    }

    foreach (var (sourceId, edge) in edgeList)
    {
        var root1 = Find(sourceId);
        var root2 = Find(edge.TargetNode.Id);
        if (root1 != root2)
        {
            mst.Add((sourceId, edge));
            Union(root1, root2);
        }
    }

    return mst;
    }

}    
