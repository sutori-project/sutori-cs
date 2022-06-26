using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace SutoriProject.Sutori
{
    public abstract class SutoriUriLoader
    {
        public static SutoriUriLoader Default { get; } = new SutoriUriLoaderBasic();

        public abstract Task<string> LoadUriAsync(string uri);
    }


    public sealed class SutoriUriLoaderBasic : SutoriUriLoader
    {

        public async override Task<string> LoadUriAsync(string uri)
        {
            if (uri.Contains("http"))
            {
                WebClient wc = new WebClient();
                return await wc.DownloadStringTaskAsync(uri);
            }
            
            string xml = "";
            if (!File.Exists(uri))
            {
                throw new FileNotFoundException($"{uri} is missing.");
            }
            using (var reader = File.OpenText(uri))
            {
                xml = await reader.ReadToEndAsync();
            }
            return xml;
        }
    }
}