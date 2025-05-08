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
         if (graphData.forDijsktra != null)
        {
            var deneme = graph.Dijkstra(graphData.forDijsktra);
            foreach (var d in deneme)
            {
                Console.WriteLine($"{d.Key} ->" + d.Value);

            }
        }


        return Ok(new {
            message = "Graf başarıyla alındı",
            nodeCount = NodeSayisi,
            edgeCount = KenarSayisi
        });
    }
}
