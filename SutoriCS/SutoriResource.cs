namespace SutoriProject.Sutori
{
    public abstract class SutoriResource : IAttributeContainer
    {
        /// <summary>
        /// The resource id.
        /// </summary>
        public string ID { get; set; } = null;


        /// <summary>
        /// The resource name.
        /// </summary>
        public string Name { get; set; } = "Untitled";
    }
}