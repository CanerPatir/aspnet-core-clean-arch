using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Infrastructure.AspNet
{
    public static class HttpExtensions
    {
        public static async Task WriteJson<T>(this HttpResponse response, T obj, string contentType = null)
        {
            var jsonSerializer = response.HttpContext.RequestServices.GetRequiredService<IOptions<MvcJsonOptions>>().Value;
            var serializer = JsonSerializer.Create(jsonSerializer.SerializerSettings);
            serializer.NullValueHandling = NullValueHandling.Ignore;
            response.ContentType = contentType ?? "application/json";
            using (var writer = new HttpResponseStreamWriter(response.Body, Encoding.UTF8))
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    jsonWriter.CloseOutput = false;
                    jsonWriter.AutoCompleteOnClose = false;

                    serializer.Serialize(jsonWriter, obj);
                    await writer.FlushAsync().ConfigureAwait(false);
                }
            }
        }
    }
}