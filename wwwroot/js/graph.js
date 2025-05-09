

var cy = cytoscape({    // cytoscape üzerinden oluşturduğumuz düğüm ve kenarları tutmamıza ve ekrana çizmemize yarar.
    container: document.getElementById('cy'),
    elements: [],
    style: [
        { selector: 'node', style: { 'label': 'data(id)', 'background-color': '#0074D9' } },
        {
            selector: 'edge', style: {
                'width': 3,
                'line-color': '#ccc',
                'target-arrow-color': '#ccc',
                'target-arrow-shape': 'none',
                'curve-style': 'bezier',
                'label': 'data(weight)', // ağırlık değerini kenarın üzerine yazmak için 
                'font-size': 12,
                'text-rotation': 'autorotate' // Etiketi kenara göre döndür
            }
        }
    ]
});
//boş bir yere düğüm ekleme fonksiyonu
function handleRightClick(event) {
    if (event.target === cy) {
        var id = prompt("Yeni Düğüm ID'si:");
        if (id) {
            cy.add({ group: 'nodes', data: { id: id }, position: event.position });
        }
    }
}
//mouse sağ tık dinleyicisi'cxxtap' 
cy.on('cxttap', handleRightClick);

// İki tıklama sonunda  edge oluşturma
let sourceNode = null;
// edge ekleme 
function addEdge(event) {
    var isDirectedGraph = document.getElementById('directedGraph').checked; // graphın yönlü-yönsüz olması için kontrol değişkeni .
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
                        },
                    }).style('target-arrow-shape', isDirectedGraph ? 'triangle' : 'none');
                }
            }
            // Renkleri sıfırla
            sourceNode.style('background-color', '#0074D9');
            sourceNode = null;
        }
        selectNodeforDjkstra()
    }
}
// mouse sol tık dinleyicisi
cy.on('tap', addEdge);


// Node veya Edge Silme  -> klavyeden gelen d nodeları, e ise kenarları silmek için
document.addEventListener('keydown', function (event) {
    if (event.key === 'n') {  // 'n' tuşuna basıldığında Node ekleme moduna geç
        mode = 'node';
        var deleteNode = prompt("Silmek istediğiniz Node");
        var nodeExists = cy.elements('#' + deleteNode).length > 0;
        if (nodeExists) {
            var nodeToDelete = cy.getElementById(deleteNode);  // Node'u al
            nodeToDelete.remove();
        } else {
            console.log("geçersiz Id girildi.")
        }
    }
    if (event.key === 'e') {
        var sourceNodeId = prompt("Silmek istediğiniz Edge'in Source Node ID'sini girin:");
        var targetNodeId = prompt("Silmek istediğiniz Edge'in Target Node ID'sini girin:");

        var edgeExists = cy.edges(`[source = '${sourceNodeId}'][target = '${targetNodeId}']`).length > 0;

        if (edgeExists) {
            var edgeToDelete = cy.edges(`[source = '${sourceNodeId}'][target = '${targetNodeId}']`);
            edgeToDelete.remove();  // Edge'i sil
            console.log("Edge başarıyla silindi.");
        } else {
            console.log("Geçersiz Source veya Target ID'si.");
        }
    }
});
function selectNodeforDjkstra() {
    const selectElement = document.getElementById("myStartNodeDjkstra");
    selectElement.innerHTML = '';
    const defaultOption = document.createElement("option");
    defaultOption.value = '';
    defaultOption.textContent = 'Seçim Yapınız';
    defaultOption.selected = true; // Varsayılan olarak seçili olsun
    defaultOption.disabled = true;
    selectElement.appendChild(defaultOption);
    cy.nodes().forEach(node => {
        const option = document.createElement("option");
        option.value = node.id();  // Seçilen node'un ID'si
        option.textContent = node.id();  // Seçilen node'un label'ı (ID olarak kullanılıyor)
        selectElement.appendChild(option);  // Select'e option ekle
    });
    // Her node için bir option oluştur ve select'e ekle

}


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
    var isDirected = document.getElementById('directedGraph').checked; // graphın yönlü-yönsüz özelliğini öğrenmemize olanak sağlar.
    var forDijsktraValue = document.getElementById('myStartNodeDjkstra').value; // Dijkstra algo. için
    var forDijsktra = forDijsktraValue === '' ? null : forDijsktraValue;
    return { nodes: nodes, edges: edges, isDirected: isDirected, forDijsktra: forDijsktra };
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

            const resultDiv = document.getElementById('result');         // ekrana backend kısmında işlenen verinin yazılması için bir deneme.
            resultDiv.innerHTML = `
            <p>${data.message}</p>
            <p>Node Sayısı: ${data.nodeCount}</p>
            <p>Edge Sayısı: ${data.edgeCount}</p> `;

            var dijkstraSonucYaz = document.getElementById('dijkstraSonuc');    // dijkstra sonucu eğer var ise
            if (data.dijkstraSonuc) {
                let dijkstraResultString = '';

                // Dijkstra sonucunu bir string olarak işleyelim:
                for (const key in data.dijkstraSonuc) {
                    if (data.dijkstraSonuc.hasOwnProperty(key)) {
                        dijkstraResultString += `<p>${key} -> ${data.dijkstraSonuc[key]}</p>`;
                    }
                }

                // Dijkstra sonucunu ekrana yazdıralım
                dijkstraSonucYaz.innerHTML = `
            <p>Dijkstra Sonuçları:</p>
            ${dijkstraResultString}
        `;
            } 

        })
        .catch(error => {
            console.error('Gönderim hatası:', error);
        });
}

function cleanTheElements() {
    cy.elements().remove();
    document.getElementById('result').innerText = null;
    document.getElementById("myStartNodeDjkstra").innerText = null;
    if (document.getElementById('directedGraph').checked) {
        document.getElementById('directedGraph').checked = false;
    }
    document.getElementById('dijkstraSonuc').innerText = null;
}




