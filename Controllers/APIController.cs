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
         Dictionary<string, int> deneme = null;
         if (graphData.forDijsktra != null)
        {
            deneme = graph.Dijkstra(graphData.forDijsktra);
            foreach (var d in deneme)
            {
                Console.WriteLine($"{d.Key} ->" + d.Value);

            }
        }
        Console.WriteLine($"{graphData.isPrim} prim  degeri"); // kontrol için yazdırıyorum
        if(graphData.isPrim)
        {
            var primDeneme = graph.PrimMST("a");
            foreach(var a in primDeneme)
            {
                Console.WriteLine($"{a.from} - {a.to} -> {a.weight}");
            }
        }


        return Ok(new {
            message = "Graf başarıyla alındı",
            nodeCount = NodeSayisi,
            edgeCount = KenarSayisi,
            dijkstraSonuc = deneme
        });
    }
}
