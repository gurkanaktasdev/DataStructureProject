using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.ObjectPool;
using Microsoft.VisualBasic;

namespace veriYapilari.Models;


public class Graph      // json formatında aldığımız verileri grah veri türüne çeviriyoruz . 
{   
    public bool isDirectedGraph{get; set;} = false;
    public string forDjsktra {get; set;} = null; // djkstra için başlangı düğümü seçilir 
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
    // Prim Algoritması (Minimum Spanning Tree)
    public List<(string source, string target, int weight)> Prim()
    {
        var mstEdges = new List<(string source, string target, int weight)>();
        var visited = new HashSet<string>();
        var priorityQueue = new SortedSet<(int weight, string source, string target)>();

        // Başlangıç olarak herhangi bir düğüm seçiyoruz (ilk düğüm)
        var startNode = AdjacencyList.Keys.First();

        // Başlangıç düğümünün komşularını ekliyoruz
        foreach (var (neighbor, weight) in AdjacencyList[startNode])
        {
            priorityQueue.Add((weight, startNode, neighbor));
        }

        visited.Add(startNode);

        // Prim algoritması çalıştırıyoruz
        while (priorityQueue.Count > 0)
        {
            // En düşük maliyetli kenarı seçiyoruz
            var (weight, source, target) = priorityQueue.Min;
            priorityQueue.Remove(priorityQueue.Min());

            // Eğer hedef düğüm zaten ziyaret edildiyse, geçiyoruz
            if (visited.Contains(target)) continue;

            // Kenarı MST'ye ekliyoruz
            mstEdges.Add((source, target, weight));
            visited.Add(target);

            // Yeni ziyaret edilen düğümün komşularını ekliyoruz
            foreach (var (neighbor, edgeWeight) in AdjacencyList[target])
            {
                if (!visited.Contains(neighbor))
                {
                    priorityQueue.Add((edgeWeight, target, neighbor));
                }
            }
        }

        return mstEdges;
    }

}
