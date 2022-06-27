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


        #region Non Async Equivalents

        [Obsolete("Please use the await version instead.")] public void GotoMomentID(string momentID) => GotoMomentIDAsync(momentID).GetAwaiter().GetResult();
        [Obsolete("Please use the await version instead.")] public void GotoMoment(SutoriMoment moment) => GotoMomentAsync(moment).GetAwaiter().GetResult();
        [Obsolete("Please use the await version instead.")] public void Play() => PlayAsync().GetAwaiter().GetResult();
        [Obsolete("Please use the await version instead.")] public void GotoNextMoment() => GotoNextMomentAsync().GetAwaiter().GetResult();

        #endregion


        /// <summary>
        /// Goto a specific moment by id.
        /// </summary>
        /// <param name="momentID"></param>
        public async Task GotoMomentIDAsync(string momentID)
        {
            SutoriMoment moment = Document.Moments.Find(t => t.ID == momentID);
            if (moment == null) throw new Exception("Could not find moment with id #{momentID}.");
            await GotoMomentAsync(moment);
        }


        /// <summary>
        /// Goto a specific moment.
        /// </summary>
        /// <param name="moment"></param>
        public async Task GotoMomentAsync(SutoriMoment moment)
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
                    string incXML = await Document.UriLoader.LoadUriAsync(loadElement.Path);
                    await Document.LoadXmlParts(incXML, loadElement.Path, true);
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
            await GotoMomentAsync(null);
        }


        /// <summary>
        /// Go to the next logical moment. The next sequential moment is selected, unless
        /// the current moment has a goto option, which will be used instead if found.
        /// </summary>
        /// <returns>True if successful.</returns>
        public async Task<bool> GotoNextMomentAsync()
        {
            if (Cursor == null) return false; // no cursor present.
            int index = Document.Moments.IndexOf(Cursor);
            if (index == -1) return false; // cursor doesn't belong to document.

            // if the moment has a goto, use that instead.
            if (!string.IsNullOrWhiteSpace(Cursor.Goto))
            {
                await GotoMomentIDAsync(Cursor.Goto);
                return false;
            }

            if (index == Document.Moments.Count - 1)
            {
                Ended = true;
                return false; // end of sequence.
            }
            await GotoMomentAsync(Document.Moments[index + 1]);
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