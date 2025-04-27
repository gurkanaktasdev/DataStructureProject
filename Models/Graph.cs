namespace veriYapilari.Models;


public class Graph      // json formatında aldığımız verileri grah veri türüne çeviriyoruz . 
{
    public Dictionary<string , List<(string target, int weight)>> AdjacencyList { get; set; }

    public Graph(GraphData graphData)
    {
        AdjacencyList = new Dictionary<string, List<(string target, int weight)>>();

        // Bütün düğümleri ekle
        foreach (var node in graphData.Nodes)
        {
            AdjacencyList[node.Id] = new List<(string, int)>();
        }

        // Kenarları ekle
        foreach (var edge in graphData.Edges)
        {
            AdjacencyList[edge.Source].Add((edge.Target, edge.Weight));
        }
    }

}