using System.Xml.Linq;

namespace SutoriProject.Sutori.Elements
{
    public sealed class SutoriElementSet : SutoriElement
    {
        public string Name { get; set; } = "";
        public string Value { get; set; } = "";


        public static SutoriElementSet Parse(XElement element)
        {
            SutoriElementSet result = new SutoriElementSet();
            result.Name = element.AttributeAsString("name");
            result.Value = element.Value;
            result.ParseExtraAttributes(element, "name");
            return result;
        }
    }
}