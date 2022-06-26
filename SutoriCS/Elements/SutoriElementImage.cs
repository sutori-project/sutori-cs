using System.Xml.Linq;

namespace SutoriProject.Sutori.Elements
{
    public sealed class SutoriElementImage : SutoriElement
    {
        public string Actor { get; set; } = null;
        public string For { get; set; } = null;
        public string ResourceID { get; set; } = null;


        public static SutoriElementImage Parse(XElement element)
        {
            SutoriElementImage result = new SutoriElementImage();
            result.ContentCulture = element.AttributeAsCulture("lang");
            result.Actor = element.AttributeAsString("actor");
            result.For = element.AttributeAsString("for");
            result.ResourceID = element.AttributeAsString("resource");
            result.ParseExtraAttributes(element, "lang", "actor", "for", "resource");
            return result;
        }
    }
}