using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Formatting;

namespace ConsoleAppRefuerzo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Instanciar el cliente HttpClient dentro del espacio de nombres System.net.Http (using)
            var http = new HttpClient();

            //Iniciar la comunicación GET, POST, PUT, DELETE utilizando el método correspondiente.
            var respuesta = http.GetAsync("http://northwind.demos.network/api/customers/ANATR?type=json").Result;

            //Analizamos el código de respuesta utilizando la propiedad StatusCode
            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Lectura y Deserialización en el mismo paso con el using System.Net.Http.Formatting;
                var client2 = respuesta.Content.ReadAsAsync<dynamic>().Result;
                Console.WriteLine("Empresa 2 : {0}", client2.CompanyName);
                Console.WriteLine("Pais 2 : {0}", client2.Country);


                //Leemos el contenido utilizando los métodos Read
                var datos = respuesta.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Contenido: {0}", datos); // Pintar con comodines
                // Console.WriteLine("Contenido: " + datos);

                //Deserializamos para convertir de JSON o XML a objeto
                var client = JsonConvert.DeserializeObject<dynamic>(datos);

                Console.WriteLine("Empresa: {0}", client.CompanyName);
                Console.WriteLine("Pais: {0}", client.Country);

            }
            else {
                Console.WriteLine("Error de HTTP: {0}", respuesta.StatusCode);
            }

            Console.ReadKey();
        }
    }
}
