using System.Xml.Linq;

namespace SutoriProject.Sutori.Elements
{
    public sealed class SutoriElementMedia : SutoriElement
    {
        public string For { get; set; } = null;
        public string ResourceID { get; set; } = null;


        public static SutoriElementMedia Parse(XElement element)
        {
            SutoriElementMedia result = new SutoriElementMedia();
            result.ContentCulture = element.AttributeAsCulture("lang");
            result.For = element.AttributeAsString("for");
            result.ResourceID = element.AttributeAsString("resource");
            result.ParseExtraAttributes(element, "lang", "actor", "for", "resource");
            return result;
        }
    }
}