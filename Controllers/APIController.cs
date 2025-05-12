using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using veriYapilari.Models;

namespace veriYapilari.Controllers;


[Route("api/graph")]
[ApiController]
public class GraphController : ControllerBase
{
    [HttpPost]
    public IActionResult PostGraph([FromBody] GraphData graphData)
    {
        int NodeSayisi = graphData.Nodes.Count();
        int KenarSayisi = graphData.Edges.Count();

        CustomGraph graph = new();

        foreach (var node in graphData.Nodes)   // komşuluk listesinin alt yapısıını oluşturduk.
        {
            graph.AddNode(node.Id!);
        }

        foreach (var edge in graphData.Edges)
        {
            graph.AddEdge(edge.Source!, edge.Target!, edge.Weight);

            if (!graphData.IsDirected)
            {
                graph.AddEdge(edge.Target!, edge.Source!, edge.Weight);
            }
        }
        graph.PrintGraph();
        Console.WriteLine("----------------");
        Dictionary<string, int> dijsktradeneme = null;
        if (graphData.forDijsktra != null)
        {
            dijsktradeneme = graph.Dijkstra(graphData.forDijsktra);
            foreach (var d in dijsktradeneme)
            {
                Console.WriteLine($"{d.Key} ->" + d.Value);

            }
        }
        Console.WriteLine("------------------");
        List<Object> primResult = null;
        if (graphData.isPrim != null)
        {
            var primDeneme = graph.PrimMST(graphData.isPrim);
            foreach (var a in primDeneme)
            {
                Console.WriteLine($"{a.from} - {a.to} -> {a.weight}");
            }
            primResult = primDeneme.Select(edge => (object)new   // tuple nesnesini otmatik json a çeviremediği için böyle bir yol izledim
            {
                from = edge.Item1,
                to = edge.Item2,
                weight = edge.Item3
            }).ToList();
        }

        Console.WriteLine($"Kruskal sonucu:{graphData.isKruskal}");
        List<object> kruskalResult = null;
        if (graphData.isKruskal == true)
        {
            var kruskalDeneme = graph.Kruskal();
            foreach (var (source, edge) in kruskalDeneme)
            {
                Console.WriteLine($"{source} -> {edge.TargetNode.Id} (Weight: {edge.Weight})");
            }
            kruskalResult = kruskalDeneme.Select(edge => (object)new
            {
                from = edge.SourceId,
                to = edge.Edge.TargetNode.Id,
                weight = edge.Edge.Weight
            }).ToList();


        }
        return Ok(new
        {
            message = "Graf başarıyla alındı",
            nodeCount = NodeSayisi,
            edgeCount = KenarSayisi,
            dijkstraSonuc = dijsktradeneme,
            primSonuc = primResult,
            kruskalSonuc = kruskalResult

        });
    }
}

