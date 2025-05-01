using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.ObjectPool;
using Microsoft.VisualBasic;

namespace veriYapilari.Models;


public class Graph      // json formatında aldığımız verileri grah veri türüne çeviriyoruz . 
{   
    public bool isDirectedGraph{get; set;} = false;
    public string forDjsktra {get; set;} = null;
    public Dictionary<string, List<(string target, int weight)>> AdjacencyList { get; set; }

    public Graph(GraphData graphData)   // json olarak alınan veriler işleniyor ek olarak Adjacenylist oluşturuluyor .
    {
        this.isDirectedGraph = graphData.IsDirected;
        this.forDjsktra = graphData.forDijsktra;
        AdjacencyList = new Dictionary<string, List<(string target, int weight)>>();

        // Bütün düğümleri eklee
        foreach (var node in graphData.Nodes)
        {
            AdjacencyList[node.Id] = new List<(string, int)>();
        }
        
        // Kenarları ekle
        foreach (var edge in graphData.Edges)
        {
            AdjacencyList[edge.Source].Add((edge.Target, edge.Weight));
            // Eğer yönsüzse ters yönü de ekle
            if (!graphData.IsDirected)
            {
                AdjacencyList[edge.Target].Add((edge.Source, edge.Weight));
            }
        }
    }
    // Dijkstra Algoritması
    public Dictionary<string, int> Dijkstra(string start)
    {
        var distances = new Dictionary<string, int>();
        var priorityQueue = new SortedSet<(int distance, string node)>();
        var visited = new HashSet<string>();

        foreach (var node in AdjacencyList.Keys)
        {
            distances[node] = int.MaxValue;
        }

        distances[start] = 0;
        priorityQueue.Add((0, start));

        while (priorityQueue.Count > 0)
        {
            var (currentDistance, currentNode) = priorityQueue.Min;
            priorityQueue.Remove(priorityQueue.Min);

            if (visited.Contains(currentNode))
                continue;

            visited.Add(currentNode);

            foreach (var (neighbor, weight) in AdjacencyList[currentNode])
            {
                int newDist = currentDistance + weight;
                if (newDist < distances[neighbor])
                {
                    distances[neighbor] = newDist;
                    priorityQueue.Add((newDist, neighbor));
                }
            }
        }

        return distances;
    }

}
