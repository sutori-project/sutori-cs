using SutoriProject.Sutori.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SutoriProject.Sutori
{
    /// <summary>
    /// Describes the .NET representation of a Sutori document.
    /// </summary>
    public class SutoriDocument
    {
        /// <summary>
        /// Any associated properties.
        /// </summary>
        public Dictionary<string, string> Properties { get; private set; }

        /// <summary>
        /// Contains loaded resources (images, audio, video etc...)
        /// </summary>
        public List<SutoriResource> Resources { get; private set; }

        /// <summary>
        /// Actors associated with this document.
        /// </summary>
        public List<SutoriActor> Actors { get; private set; }

        /// <summary>
        /// Sequential list of moments.
        /// </summary>
        public List<SutoriMoment> Moments { get; private set; }

        /// <summary>
        /// List of links to other XML sutori documents.
        /// </summary>
        public List<SutoriInclude> Includes { get; private set; }

        /// <summary>
        /// The loading mechanism used when including external XML files.
        /// </summary>
        public SutoriUriLoader UriLoader { get; set; }


        /// <summary>
        /// Create a new SutoriDocument.
        /// </summary>
        public SutoriDocument()
        {
            Properties = new Dictionary<string, string>();
            Resources = new List<SutoriResource>();
            Actors = new List<SutoriActor>();
            Moments = new List<SutoriMoment>();
            Includes = new List<SutoriInclude>();
            UriLoader = SutoriUriLoader.Default;
        }


        /// <summary>
        /// Load a SutoriDocument from an XML file located at a uri. The uri can be remote or local.
        /// </summary>
        /// <param name="uri">An either remote or local uri.</param>
        /// <param name="load_includes">Set to true if includes should be loaded when calling this method.</param>
        /// <returns>The loaded SutoriDocument instance.</returns>
        public static async Task<SutoriDocument> LoadFromXml(string uri, bool load_includes = true)
        {
            return await LoadFromXml(uri, SutoriUriLoader.Default, load_includes);
        }


        /// <summary>
        /// Load a SutoriDocument from an XML file located at a uri. The uri can be remote or local.
        /// </summary>
        /// <param name="uri">An either remote or local uri.</param>
        /// <param name="loader"></param>
        /// <param name="load_includes">Set to true if includes should be loaded when calling this method.</param>
        /// <returns>The loaded SutoriDocument instance.</returns>
        public static async Task<SutoriDocument> LoadFromXml(string uri, SutoriUriLoader loader, bool load_includes = true)
        {
            SutoriDocument result = new SutoriDocument();
            result.UriLoader = loader;
            await result.LoadXmlParts(uri, load_includes);
            return result;
        }


        /// <summary>
        /// Load document parts from an XML file, and place them at the end of the current document.
        /// </summary>
        /// <param name="uri">The URI location of the XML file to load.</param>
        /// <param name="load_includes">Weather or not to load the found includes.</param>
        internal async Task LoadXmlParts(string uri, bool load_includes)
        {
            string xml = await UriLoader.LoadUriAsync(uri);
            XDocument doc = XDocument.Parse(xml);
            XElement docElement = doc.Element("document");

            // load properties.
            XElement propsElement = docElement.Element("properties");
            if (propsElement != null)
            {
                foreach (XElement propElement in propsElement.Elements())
                {
                    string key = propElement.Name.LocalName;
                    if (!Properties.ContainsKey(key))
                        Properties.Add(key, propElement.Value);
                    else
                        Properties[key] = propElement.Value;
                }
            }

            // load includes.
            foreach (XElement incElement in docElement.Elements("include"))
            {
                SutoriInclude include = SutoriInclude.Parse(incElement);
                Includes.Add(include);
                if (include.After == false && load_includes)
                {
                    await LoadXmlParts(include.Path, load_includes);
                    include.Loaded = true;
                }
            }

            // load resources.
            XElement resourcesElement = docElement.Element("resources");
            if (resourcesElement != null)
            {
                foreach (XElement resElement in resourcesElement.Elements())
                {
                    if (resElement.Name.LocalName == "image")
                    {
                        Resources.Add(SutoriResourceImage.Parse(resElement));
                    }
                }
            }

            // load actors.
            XElement actorsElement = docElement.Element("actors");
            if (actorsElement != null)
            {
                foreach (XElement actorElement in actorsElement.Elements("actor"))
                {
                    Actors.Add(SutoriActor.Parse(actorElement));
                }
            }

            // load moments.
            XElement momentsElement = docElement.Element("moments");
            if (momentsElement != null)
            {
                foreach (XElement momentElement in momentsElement.Elements("moment"))
                {
                    Moments.Add(SutoriMoment.Parse(momentElement));
                }
            }

            // load includes that after set to true.
            foreach (SutoriInclude include in this.Includes)
            {
                if (include.After && load_includes)
                {
                    await LoadXmlParts(include.Path, load_includes);
                    include.Loaded = true;
                }
            }
        }


        /// <summary>
        /// Get a resource by ID.
        /// </summary>
        /// <typeparam name="T">The type of resource expected (for example SutoriResourceImage).</typeparam>
        /// <param name="id">The ID of the resource.</param>
        public T GetResourceById<T>(string id) where T : SutoriResource
        {
            return Resources.Find(t => t.ID == id) as T;
        }
    }
}