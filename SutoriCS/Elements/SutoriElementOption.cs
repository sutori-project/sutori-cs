using SutoriProject.Sutori.Enums;
using System.Xml.Linq;

namespace SutoriProject.Sutori.Elements
{
    public sealed class SutoriElementOption : SutoriElement
    {
        public string Text { get; set; } = "";
        public string Target { get; set; } = "";
        public SutoriSolver Solver { get; set; } = SutoriSolver.None;
        public string SolverCallback { get; set; } = "";


        public static SutoriElementOption Parse(XElement element)
        {
            SutoriElementOption result = new SutoriElementOption();
            result.Text = element.Value;
            result.Target = element.AttributeAsString("target");
            result.Solver = element.AttributeAsSolver("solver");
            result.SolverCallback = element.AttributeAsString("solver_callback");
            result.ContentCulture = element.AttributeAsCulture("lang");
            result.ParseExtraAttributes(element, "target", "solver", "solver_callback", "lang");
            return result;
        }
    }
}