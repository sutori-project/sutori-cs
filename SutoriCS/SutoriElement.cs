using SutoriProject.Sutori.Enums;

namespace SutoriProject.Sutori
{
    public abstract class SutoriElement : IAttributeContainer
    {
        public SutoriCulture ContentCulture { get; set; }
    }
}