using System;

namespace SutoriProject.Sutori.Attributes
{
    internal class AlternativeAttribute : Attribute
    {
        public string Alternative { get; set; }

        public AlternativeAttribute(string alternative)
        {
            Alternative = alternative;
        }
    }
}