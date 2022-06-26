using System.Xml.Linq;

namespace SutoriProject.Sutori
{
    public class SutoriInclude
    {
        public string Path { get; set; } = "";

        public bool After { get; set; } = false;


        public bool Loaded { get; set; } = false;


        internal static SutoriInclude Parse(XElement element)
        {
            SutoriInclude result = new SutoriInclude();
            result.Path = element.Value;

            XAttribute afterAttrib = element.Attribute("after");
            if (afterAttrib != null)
            {
                result.After = afterAttrib.Value == "true";
            }

            return result;
        }
    }
}