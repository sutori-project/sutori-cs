using System.Xml.Linq;

namespace SutoriProject.Sutori.Elements
{
    public sealed class SutoriElementLoad : SutoriElement
    {
        public string Path { get; set; } = "";
        public bool Loaded { get; set; } = false;


        public static SutoriElementLoad Parse(XElement element)
        {
            SutoriElementLoad result = new SutoriElementLoad();
            result.Path = element.Value;
            result.ParseExtraAttributes(element);
            return result;
        }
    }
}