using SutoriProject.Sutori.Enums;
using System.Linq;
using System.Xml.Linq;

namespace SutoriProject.Sutori
{
    internal static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string AttributeAsString(this XElement element, string attributeName, string defaultValue = null)
        {
            XAttribute attribute = element.Attribute(attributeName);
            if (attribute == null) return defaultValue;
            return attribute.Value ?? defaultValue;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool AttributeAsBool(this XElement element, string attributeName, bool defaultValue = false)
        {
            XAttribute attribute = element.Attribute(attributeName);
            if (attribute == null) return defaultValue;
            return bool.Parse(attribute.Value);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static SutoriCulture AttributeAsCulture(this XElement element, string attributeName, SutoriCulture defaultValue = SutoriCulture.None)
        {
            XAttribute attribute = element.Attribute(attributeName);
            if (attribute == null) return defaultValue;
            return SutoriCultureHelper.Parse(attribute.Value);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static SutoriSolver AttributeAsSolver(this XElement element, string attributeName, SutoriSolver defaultValue = SutoriSolver.None)
        {
            XAttribute attribute = element.Attribute(attributeName);
            if (attribute == null) return defaultValue;
            return SutoriSolverHelper.Parse(attribute.Value);
        }


        /// <summary>
        /// Parse extra attributes when parsing an element.
        /// </summary>
        /// <param name="element">The source element.</param>
        /// <param name="exclude">An array of keys to exclude.</param>
        public static void ParseExtraAttributes(this IAttributeContainer conainer, XElement element, params string[] exclude)
        {
            foreach (XAttribute attrib in element.Attributes())
            {
                string key = attrib.Name.LocalName;
                if (exclude.Contains(key)) continue;
                conainer.Attributes[key] = attrib.Value;
            }
        }
    }
}