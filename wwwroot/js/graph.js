var cy = cytoscape({
    container: document.getElementById('cy'),
    elements: [],
    style: [
        { selector: 'node', style: { 'label': 'data(id)', 'background-color': '#0074D9' } },
        { selector: 'edge', style: { 
            'width': 3, 
            'line-color': '#ccc', 
            'target-arrow-color': '#ccc', 
            'target-arrow-shape': 'triangle',
            'curve-style': 'bezier',
            'label': 'data(weight)', // ağırlık değerini kenarın üzerine yazmak için 
            'font-size': 12,
            'text-rotation': 'autorotate' // Etiketi kenara göre döndür
        }}
    ]
});
// Sağ tıkla boş yere düğüm ekleme
cy.on('cxttap', function(event) {
    if (event.target === cy) {
        var id = prompt("Yeni Düğüm ID'si:");
        if (id) {
            cy.add({ group: 'nodes', data: { id: id }, position: event.position });
            
        }
    }
    /*var elements = cy.elements();                 ->  Web üzerinden log çıktılarını kontrol ettim .
    elements.forEach(function(element) {
    console.log('Öğe:', element.id()); // Öğenin ID'sini yazdırır
    console.log('Veri:', element.data()); // Öğenin tüm verisini yazdırır
    console.log('Tür:', element.group);
});*/
});
// backande kısmını dinamik oluşturduğumuz graph veriyapısını düzenlememize olanak sağlar.
function exportGraphData() {
    var nodes = cy.nodes().map(node => ({
        id: node.id()
    }));

    var edges = cy.edges().map(edge => ({
        source: edge.data('source'),
        target: edge.data('target'),
        weight: edge.data('weight')
    }));

    return { nodes: nodes, edges: edges };
}
// bu da yukarıdaki fonksiyon ile düzenlediğimiz veriyapısını backande post etmemize yarar
function sendGraphToServer() {
    var graphData = exportGraphData();  // graph verisini al

    fetch('/api/graph', {   // Backend'deki API endpoint'ine post
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(graphData)
    })
    .then(response => {
        if (response.ok) {
            return response.json();
        }
        throw new Error('Ağ hatası');
    })
    .then(data => {
        console.log('Sunucudan gelen cevap:', data);

        const resultDiv = document.getElementById('result'); // ekrana backend kısmında işlenen verinin yazılması için bir deneme.
        resultDiv.innerHTML = `
            <p>${data.message}</p>
            <p>Node Sayısı: ${data.nodeCount}</p>
            <p>Edge Sayısı: ${data.edgeCount}</p>
        `;
    })
    .catch(error => {
        console.error('Gönderim hatası:', error);
    });
}



// İki tıklama ile edge oluşturma
let sourceNode = null;

cy.on('tap', function(event) {
    if (event.target.group && event.target.group() === 'nodes') {
        if (sourceNode == null) {
            // İlk tıklanan düğüm source olacak
            sourceNode = event.target;
            sourceNode.style('background-color', '#FF4136'); // seçildiğini belli et
        } else {
            // İkinci düğüm target olacak
            var targetNode = event.target;
            if (sourceNode.id() !== targetNode.id()) {      // burada arka arkaya aynı düğüm seçilmesin diye konumuş bir koşul
                var weight = prompt("Edge ağırlığı (sayı gir):", "1");
                if (weight != null) {
                    cy.add({
                        group: 'edges',
                        data: { 
                            source: sourceNode.id(), 
                            target: targetNode.id(),
                            weight: parseInt(weight)
                        }
                    });
                }
            }
            // Renkleri sıfırla
            sourceNode.style('background-color', '#0074D9');
            sourceNode = null;
        }
    }
});


