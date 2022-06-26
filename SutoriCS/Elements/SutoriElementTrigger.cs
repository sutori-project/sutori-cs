using System.Xml.Linq;

namespace SutoriProject.Sutori.Elements
{
    public sealed class SutoriElementTrigger : SutoriElement
    {
        public string Action { get; set; } = "";
        public string Body { get; set; } = "";


        public static SutoriElementTrigger Parse(XElement element)
        {
            SutoriElementTrigger result = new SutoriElementTrigger();
            result.Action = element.AttributeAsString("action");
            result.Body = element.Value;
            result.ParseExtraAttributes(element, "action");
            return result;
        }
    }
}