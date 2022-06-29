using SutoriProject.Sutori.Elements;
using SutoriProject.Sutori.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SutoriProject.Sutori
{
    public class SutoriMoment : IAttributeContainer
    {
        public string Actor { get; set; } = null;
        public bool Clear { get; set; } = false;
        public string Goto { get; set; } = null;
        public string ID { get; set; } = null;
        public List<SutoriElement> Elements { get; set; } = new List<SutoriElement>();


        internal static SutoriMoment Parse(XElement element)
        {
            SutoriMoment result = new SutoriMoment();

            result.Actor = element.AttributeAsString("actor");
            result.Clear = element.AttributeAsBool("clear", false);
            result.Goto = element.AttributeAsString("goto");
            result.ID = element.AttributeAsString("id");
            result.ParseExtraAttributes(element, "actor", "clear", "goto", "id");

            foreach (XElement childElement in element.Elements())
            {
                switch (childElement.Name.LocalName)
                {
                    case "text": result.Elements.Add(SutoriElementText.Parse(childElement)); break;
                    case "option": result.Elements.Add(SutoriElementOption.Parse(childElement)); break;
                    case "media": result.Elements.Add(SutoriElementMedia.Parse(childElement)); break;
                    case "load": result.Elements.Add(SutoriElementLoad.Parse(childElement)); break;
                    case "set": result.Elements.Add(SutoriElementSet.Parse(childElement)); break;
                    case "trigger": result.Elements.Add(SutoriElementTrigger.Parse(childElement)); break;
                }
            }

            return result;
        }
    

        public SutoriElementText AddText(string text, SutoriCulture culture = SutoriCulture.None)
        {
            SutoriElementText element = new SutoriElementText();
            element.Text = text;
            element.ContentCulture = culture;
            Elements.Add(element);
            return element;
        }


        public SutoriElementMedia AddMedia(string resourceID, SutoriCulture culture = SutoriCulture.None)
        {
            SutoriElementMedia media = new SutoriElementMedia();
            media.ResourceID = resourceID;
            media.ContentCulture = culture;
            Elements.Add(media);
            return media;
        }


        public SutoriElementOption AddOption(string text, string target, SutoriCulture culture = SutoriCulture.None)
        {
            SutoriElementOption option = new SutoriElementOption();
            option.Text = text;
            option.Target = target;
            option.ContentCulture = culture;
            Elements.Add(option);
            return option;
        }


        /// <summary>
        /// Remove elements of a specific type and culture.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="culture"></param>
        public void RemoveElements<T>(SutoriCulture culture) where T : SutoriElement
        {
            SutoriElement[] removables = Elements.Where(t => t is T && t.ContentCulture == culture).ToArray();
            foreach (SutoriElement removable in removables)
            {
                Elements.Remove(removable);
            }            
        }


        public IEnumerable<T> GetElements<T>(SutoriCulture culture) where T :
            SutoriElement => Elements.Where(t => t is T && t.ContentCulture == culture).Select(t => t as T);


        /// <summary>
        /// Get the text for a specific culture.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public string GetText(SutoriCulture culture)
        {
            return string.Join(" ", GetElements<SutoriElementText>(culture).Select(t => t.Text));
        }
    

        /// <summary>
        /// Get the options for a specific culture.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public IEnumerable<SutoriElementOption> GetOptions(SutoriCulture culture)
        {
            return GetElements<SutoriElementOption>(culture);
        }
    }
}