using System.Xml.Linq;

namespace SutoriProject.Sutori.Elements
{
    public sealed class SutoriElementText : SutoriElement
    {
        public string Text { get; set; } = "";


        public static SutoriElementText Parse(XElement element)
        {
            SutoriElementText result = new SutoriElementText();
            result.Text = element.Value;
            result.ContentCulture = element.AttributeAsCulture("lang");
            result.ParseExtraAttributes(element, "lang");
            return result;
        }
    }
}