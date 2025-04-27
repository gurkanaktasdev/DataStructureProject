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

        var graph = new Graph(graphData);

        // Artık graph.AdjacencyList üzerinden algoritma çalıştırabiliriz
        Console.WriteLine("Graph başarıyla oluşturuldu!");

        // Örneğin Adjacency Listi yazdır:
        foreach (var node in graph.AdjacencyList)
        {
            Console.Write($"{node.Key} -> ");
            foreach (var neighbor in node.Value)
            {
                Console.Write($"({neighbor.target}, {neighbor.weight}) ");
            }
            Console.WriteLine();
        }
        return Ok(new
        {
            message = "Graf başarıyla alındı",
            nodeCount = NodeSayisi,
            edgeCount = KenarSayisi


        });
    }
}
