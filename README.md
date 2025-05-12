
**GitHub Link:** [DataStructureProject](https://github.com/gurkanaktasdev/DataStructureProject)
# 📡 Graph Ağ Simülasyonu

Bu proje, yönlü ve ağırlıklı graf yapıları üzerinde çalışmayı ve çeşitli algoritmalar (Dijkstra, BFS, DFS vb.) ile ağ üzerindeki veri akışlarını simüle etmeyi amaçlayan bir ağ simülasyonu uygulamasıdır.

---

## 📌 Özellikler

- 🎯 Düğümler (Nodes) ve kenarların (Edges) dinamik olarak tanımlanması
- 📈 Ağırlıklı ve yönlü grafik desteği
- 🔍 Dijkstra algoritması ile en kısa yol hesaplama
- 🔄 Prim ve Kruskal ile ağ üzerinde analiz
- 🖼️ Cytoscape.js ile grafiksel ağ görselleştirmesi  
- 💡 Gerçek zamanlı veri güncelleme ve yol gösterimi

---
##🖱️ Etkileşimli kullanıcı arayüzü
- 🖱️ **Sağ Tık (Right Click)**: Boş alana sağ tıklayarak yeni bir **düğüm** eklenir.
- 🖱️ **Sol Tık (Left Click)**: Sırayla iki düğüme sol tıklanarak aralarına **kenar** eklenir.
- ⌨️ **N Tuşu**: Seçilen düğüm silinir.
- ⌨️ **E Tuşu**: Seçilen kenar silinir.

> 🔔 Not: Bu etkileşimler yalnızca grafik alanı (Cytoscape.js görselleştirmesi) üzerinde geçerlidir.
---
### 🧱 Kullanılan Veri Yapıları

- **Graph**  
  Dinamik bir şekilde oluşturulan düğümleri ve kenarları anlamlı hale getirmemize olanak sağlar.

- **Priority Queue (Öncelikli Kuyruk)**  
  Dijkstra algoritması gibi kısa yol bulma işlemlerinde, minimum mesafeye sahip düğümü hızlıca seçmek için kullanılır.

- **HashSet\<string\>**  
  Ziyaret edilen düğümlerin kaydını tutarak tekrar ziyaretleri engeller.

- **Dictionary\<string, int\>**  
  Düğümlerin mesafe veya ağırlık bilgilerini saklamak için kullanılır.

---

## 🚀 Kurulum

#🛠️ Kullanılan Teknolojiler
- `C#`
- `.NET 9 / ASP.NET Core MVC`
- `JavaScript` (Cytoscape.js ile grafik çizimi)
- `HTML/CSS` + `Bootstrap`
---
1. Projeyi klonlayın:

   ```bash
   git clone https://github.com/gurkanaktasdev/DataStructureProject.git 
2. Projeyi Başlat
    ```
    dotnet restore
    dotnet run
3. Tarayıcıda AÇ
   http://localhost:5200 
   > 🔔 Not: Eğer Sizin sisteminizde 5200 portu uygun değil ise "Properties/launchSettings.json" dosyasına giderek "applicationUrl" kısmından ilgili değişiklikleri yapabilirsiniz.
