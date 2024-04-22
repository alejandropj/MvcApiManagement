using MvcApiManagement.Models;
using System.Net.Http.Headers;
using System.Web;

namespace MvcApiManagement.Services
{
    public class ServiceApiManagement
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApiEmpleados;
        private string UrlApiDepartamentos;

        public ServiceApiManagement(IConfiguration config)
        {
            this.Header = new MediaTypeWithQualityHeaderValue
                ("application/json");
            this.UrlApiEmpleados = config.GetValue<string>
                ("ApiUrls:ApiEmpleados");
            this.UrlApiDepartamentos = config.GetValue<string>
                ("ApiUrls:ApiDepartamentos");
        }

        public async Task<List<Empleado>>
            GetEmpleadosAsync()
        {
            using(HttpClient client = new HttpClient())
            {

                var queryString = HttpUtility.ParseQueryString(string.Empty);
                string request = "data?" + queryString;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.CacheControl =
                    CacheControlHeaderValue.Parse("no-cache");

                HttpResponseMessage response =
                    await client.GetAsync(this.UrlApiEmpleados + request);
                if (response.IsSuccessStatusCode)
                {
                    List<Empleado> data =
                        await response.Content.ReadAsAsync
                        <List<Empleado>>();
                    return data;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<Departamento>>
            GetDepartamentosAsync(string suscripcion)
        {
            using (HttpClient client = new HttpClient())
            {
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                string request = "api/departamentos?" + queryString;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.CacheControl =
                    CacheControlHeaderValue.Parse("no-cache");

                client.DefaultRequestHeaders.Add
                    ("Ocp-Apim-Subscription-Key", suscripcion);

                HttpResponseMessage response =
                    await client.GetAsync(this.UrlApiDepartamentos + request);
                if (response.IsSuccessStatusCode)
                {
                    List<Departamento> data =
                        await response.Content.ReadAsAsync
                        <List<Departamento>>();
                    return data;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
