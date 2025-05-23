namespace veriYapilari.Models;

public class GraphData      // API üzerinden iletilen json türündeki verinin model bazında karşılığı. 
{
    public List<Node>? Nodes {get;set;}
    public List<Edge>? Edges { get; set; }
    public bool IsDirected { get; set; } = true; // yönlü - Yönsüz kenar kontrolü için 
    public string? forDijsktra {get; set;} // dijkstra algoritması için başlangıç node un Id si.
    public string? isPrim {get; set;}  // prim algoritması için 
    public bool isKruskal{get; set;} = false; // Kruskal Algo seçilip seçilmediği
}
public class Node
{
    public string? Id { get; set; }
}

public class Edge
{
    public string? Source { get; set; }
    public string? Target { get; set; }
    public int Weight { get; set; }
}