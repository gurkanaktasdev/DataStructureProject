namespace veriYapilari.Models;

public class GraphData
{
    public List<Node>? Nodes {get;set;}
    public List<Edge>? Edges { get; set; }
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