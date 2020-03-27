using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Formatting;
using ConsoleAppOrdenarFiltrar.Model;

namespace ConsoleAppOrdenarFiltrar
{
    class Program
    {
        static void Main(string[] args)
        {
            var http = new HttpClient();
            var respuesta = http.GetAsync("http://northwind.demos.network/api/invoices?type=json").Result;

            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var datos = respuesta.Content.ReadAsAsync<List<Invoices>>().Result;

                //Método o funciones de agregación -> Count, Max, Min, Average y Sum
                Console.WriteLine("Números de registros: {0}", datos.Count);

                Console.WriteLine("Total Pedido 10248: {0}", datos.Where(r => r.OrderID == 10248).Sum(r => r.Quantity * r.UnitPrice).ToString("N2"));
                Console.WriteLine("Números de registros 10248: {0}", datos.Count(r => r.OrderID == 10248));
                Console.WriteLine("Números de registros 10248: {0}", datos.Where(r => r.OrderID == 10248).Count());

                Console.WriteLine("Números de registros ANTON: {0}", datos.Count(r => r.CustomerID == "ANTON"));

                Console.WriteLine("Mayor Precio: {0}", datos.Max(r => r.UnitPrice).ToString("N2"));

                //Cuantas unidades se han venido del producto 3
                Console.WriteLine("Numero de unidades vendidas Producto 3: {0}", datos.Where(r => r.ProductID == 3).Sum(r => r.Quantity));

                //Podemos filtrar también utilizando el contiene, comienza o finaliza
                var quesos = datos
                    .Where(r => r.ProductName.Contains("Queso"))
                    .Select(r => new { r.ProductID, r.ProductName})
                    .Distinct()
                    .ToList();
                //var quesos = datos.Select(r => r.ProductName.Contains("queso")).ToList(); Tambien se puede usar Select
                foreach (var p in quesos) 
                {
                    Console.WriteLine("{0} {1}", p.ProductID, p.ProductName);
                }

                var productos = datos.Where(r => r.CustomerName.StartsWith("Comida")).ToList();

                foreach (var p in productos)
                {
                    Console.WriteLine("{0}", p.CustomerName);
                }

                //Ordenación de la Información
                //  Primera regla de ordenación con OrderBy o OrderByDescending
                //  Reglas de ordenación consecutivas ThenBy o ThenByDescending
                //  SIEMPRE ORDENAR DESPUES DE FILTRAR
                //  C# => OrderBy, OrderByDescending, ThenBy y ThenByDescending
                //  JS y Python => .Sort()
                var lineas = datos
                    .Where(r => r.Country == "Spain")
                    .OrderBy(r => r.City)
                    .ThenBy(r => r.OrderID)
                    .ThenBy(r => r.OrderDate)
                    .ToList();

                var lineas2 = datos
                    .Where(r => r.Country == "Spain")
                    .OrderByDescending(r => r.City)
                    .ToList();

                //Extracción de información
                //  Para extraer información utilizamos Select
                // JS y Python => .map()
                var lineas3 = datos
                    .Where(r => r.OrderID == 10248)
                    .Select(r => new { r.ProductID, r.ProductName, r.UnitPrice, r.Quantity })
                    .ToList();

            }
            else
            {
                Console.WriteLine("Error de HTTP: {0}", respuesta.StatusCode);
            }

            Console.ReadKey();
        }
    }
}
