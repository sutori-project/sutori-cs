using System.Xml.Linq;

namespace SutoriProject.Sutori.Resources
{
    public sealed class SutoriResourceImage : SutoriResource
    {
        /// <summary>
        /// The resource id for the image data.
        /// </summary>
        public string Src { get; set; } = "";

        /// <summary>
        /// Weather or not to preload this image resource.
        /// </summary>
        public bool Preload { get; set; } = false;


        internal static SutoriResourceImage Parse(XElement element)
        {
            SutoriResourceImage result = new SutoriResourceImage();
            result.ID = element.AttributeAsString("id");
            result.Name = element.AttributeAsString("name");
            result.Src = element.AttributeAsString("src");
            result.ParseExtraAttributes(element, "id", "name", "src");
            return result;
        }
    }
}