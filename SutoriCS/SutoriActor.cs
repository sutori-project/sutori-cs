using SutoriProject.Sutori.Elements;
using SutoriProject.Sutori.Enums;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SutoriProject.Sutori
{
    /// <summary>
    /// Class that describes an actor.
    /// </summary>
    public sealed class SutoriActor : IAttributeContainer
    {
        /// <summary>The culture that this actor belongs too.</summary>
        public SutoriCulture ContentCulture { get; set; } = SutoriCulture.None;

        /// <summary>Any attached elements associated with this actor.</summary>
        public List<SutoriElement> Elements { get; private set; } = new List<SutoriElement>();

        /// <summary>The id of this actor, used to associate actors with moments.</summary>
        public string ID { get; set; } = null;

        /// <summary>The display name of this actor.</summary>
        public string Name { get; set; } = null;


        internal static SutoriActor Parse(XElement element)
        {
            SutoriActor result = new SutoriActor();
            result.ID = element.AttributeAsString("id");
            result.Name = element.AttributeAsString("name");
            result.ContentCulture = element.AttributeAsCulture("lang");
            result.ParseExtraAttributes(element, "id", "name", "lang");
            if (element.HasElements)
            {
                foreach (XElement child in element.Elements())
                {
                    switch (child.Name.LocalName)
                    {
                        case "text":
                            result.Elements.Add(SutoriElementText.Parse(child));
                            break;
                        case "media":
                            result.Elements.Add(SutoriElementMedia.Parse(child));
                            break;
                    }
                }
            }
            return result;
        }
    }
}