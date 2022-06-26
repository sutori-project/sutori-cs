using SutoriProject.Sutori;

// welcome the session.
Console.WriteLine("SutoriCS Test\n---------------------------");

// load the document.
SutoriDocument doc = await SutoriDocument.LoadFromXml("test1.xml");

// load the engine.
SutoriEngine engine = new SutoriEngine(doc);

// hook into the event system.
engine.HandleChallenge += async delegate(object? sender, SutoriEngineCallbackArgs e)
{
    // clear screen if moment has clear set.
    if (e.Moment.Clear) Console.Clear();

    // display the moment text.
    Console.WriteLine(e.Moment.GetText(engine.Culture));

    // display the options.
    var options = e.Moment.GetOptions(engine.Culture).ToArray();
    foreach (var option in options)
    {
        Console.WriteLine(option.Text);
    }

    // prompt & handle an answer.
    ConsoleKeyInfo key = Console.ReadKey(true);
    switch (key.Key)
    {
        case ConsoleKey.D1: await engine.GotoMomentID(options[0].Target); break;
        case ConsoleKey.D2: await engine.GotoMomentID(options[1].Target); break;
        default: await engine.GotoNextMoment(); break;
    }
};

// begin the game.
await engine.PlayAsync();

// wait for game to end.
while (engine.Ended == false) await Task.Delay(1000);

// end the session.
Console.WriteLine("---------------------------\nEnded");