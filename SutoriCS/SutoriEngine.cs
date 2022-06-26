using SutoriProject.Sutori.Elements;
using SutoriProject.Sutori.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SutoriProject.Sutori
{
    public sealed class SutoriEngine
    {
        public SutoriMoment Cursor { get; private set; }
        public SutoriCulture Culture { get; set; }
        public SutoriDocument Document { get; private set; }
        public event EventHandler<SutoriEngineCallbackArgs> HandleChallenge;
        public bool Ended { get; private set; } = false;


        public SutoriEngine(SutoriDocument document)
        {
            this.Cursor = null;
            this.Culture = SutoriCulture.None;
            this.Document = document;

            if (Document.Moments.Count == 0) throw new Exception("Can't load a document that has no moments!");
        }


        public async Task GotoMomentID(string momentID)
        {
            SutoriMoment moment = Document.Moments.Find(t => t.ID == momentID);
            if (moment == null) throw new Exception("Could not find moment with id #{momentID}.");
            await GotoMoment(moment);
        }


        public async Task GotoMoment(SutoriMoment moment)
        {
            if (moment == null) moment = Document.Moments.First();
            if (moment == null) throw new Exception("Document does not have any moments!");
            this.Cursor = moment;

            // execute any load elements set to encounter.
            SutoriElementLoad[] loadElements = moment.Elements.Where(t => t is SutoriElementLoad).Select(t => t as SutoriElementLoad).ToArray();
            foreach (SutoriElementLoad loadElement in loadElements)
            {
                if (loadElement.Loaded == false)
                {
                    await Document.LoadXmlParts(loadElement.Path, true);
                    loadElement.Loaded = true;
                }
            }

            HandleChallenge?.Invoke(this, new SutoriEngineCallbackArgs(this, moment));
        }


        /// <summary>
        /// Goto the first moment in the document.
        /// </summary>
        public async Task PlayAsync()
        {
            await GotoMoment(null);
        }


        /// <summary>
        /// Go to the next logical moment. The next sequential moment is selected, unless
        /// the current moment has a goto option, which will be used instead if found.
        /// </summary>
        /// <returns>True if successful.</returns>
        public async Task<bool> GotoNextMoment()
        {
            if (Cursor == null) return false; // no cursor present.
            int index = Document.Moments.IndexOf(Cursor);
            if (index == -1) return false; // cursor doesn't belong to document.

            // if the moment has a goto, use that instead.
            if (!string.IsNullOrWhiteSpace(Cursor.Goto))
            {
                await GotoMomentID(Cursor.Goto);
                return false;
            }

            if (index == Document.Moments.Count - 1)
            {
                Ended = true;
                return false; // end of sequence.
            }
            await GotoMoment(Document.Moments[index + 1]);
            return true;
        }
    }


    public class SutoriEngineCallbackArgs : EventArgs
    {
        public readonly SutoriEngine Engine;
        public readonly SutoriMoment Moment;
        public int? ElementCount { get => Moment?.Elements.Count; }

        public SutoriEngineCallbackArgs(SutoriEngine engine, SutoriMoment moment)
        {
            Engine = engine;
            Moment = moment;
        }
    }
}