# sutori-cs

A simple to use .NET Standard 2.0 dialog system for websites, apps, games and more.



## Introduction

Sutori is a dialog engine that enables you to add an easy to customise dialog
abilities to nearly anything that needs them. Here are some great examples of
use cases:

- A quiz/survey on a website.
- Custom checkout process for buying things on a web shop.
- Conversation system in computer game.
- Visual novel creation.
- Telephone switch board.

Dialog is written in XML files, with a structure that allows for multiple
languages, option branches, multimedia (images, audio, video). Dialog is
broken up into a list of moments in which the conversation can traverse.

Here is an example of a basic sutori XML document:

```xml
<?xml version="1.0" encoding="utf-8"?>
<document xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
          xmlns:xsd="http://www.w3.org/2001/XMLSchema">
   <moments>
      <moment>
         <text>Which door do you want to open?</text>
         <option target="door1">Door 1</option>
         <option target="door2">Door 2</option>
      </moment>

      <moment id="door1" clear="true" goto="end">
         <text>You picked door1</text>
      </moment>

      <moment id="door2" clear="true" goto="end">
         <text>You picked door2</text>
      </moment>

      <moment id="end">
         <text>This is the end</text>
      </moment>
   </moments>
</document>
```

Sutori closely mimics the way [CYOA (choose your own adventure)](https://en.wikipedia.org/wiki/Gamebook)
Gamebook's work, with the small difference is that at the end of each moment, the
user is asked what to do next.



## Sister Projects

- [sutori-studio](https://github.com/sutori-project/sutori-studio) - An IDE for editing Sutori XML files.
- [sutori-game](https://github.com/sutori-project/sutori-game) - A template for creating basic visual novels with sutori-js.
- [sutori-js](https://github.com/sutori-project/sutori-cs) - The JavaScript version of Sutori engine.



## This Repo

This repository is the .NET implementation of the Sutori dialog engine. It is
written in c#, and compiled into a .NET Standard 2.0 DLL binary specifically to
allow for as wide compatibility as possible (with focus on making sure it works
with Unity & UE).

The solution contains a sub project called SutoriCS_Test to demonstrate how to
use the engine. And further examples will be added in the near future to show
how to integrate into other systems.


## How To Install

Clone this repo, open the SutoriCS.sln file in a recent copy of Visual Studio,
then compile. The DLL file you'll need will appear in `SutoriCS/bin/debug/SutoriCS.dll`



## How To Use Sutori

Here's a bare bones example of how to setup a Sutori project: 


```cs
using SutoriProject.Sutori;

// load the document & engine.
SutoriDocument doc = await SutoriDocument.LoadFromXmlFileAsync("test1.xml");
SutoriEngine engine = new SutoriEngine(doc);

// handle the challenge event.
engine.HandleChallenge += async delegate(object? sender, SutoriEngineCallbackArgs e) {
   if (e.Moment.Clear) Console.Clear();

   // display the moment text.
   Console.WriteLine(e.Moment.GetText(engine.Culture));

   // display the options.
   var options = e.Moment.GetOptions(engine.Culture).ToArray();
   foreach (var option in options) {
      Console.WriteLine(option.Text);
   }

	// prompt & handle an answer.
   ConsoleKeyInfo key = Console.ReadKey(true);
   switch (key.Key) {
      case ConsoleKey.D1:
         await engine.GotoMomentIDAsync(options[0].Target);
         break;
      case ConsoleKey.D2:
         await engine.GotoMomentIDAsync(options[1].Target);
         break;
      default:
			await engine.GotoNextMomentAsync();
			break;
    }
};

// begin the game.
await engine.PlayAsync();

// wait for game to end.
while (engine.Ended == false) await Task.Delay(1000);
```


## Conclusion

This was created originally to figure out how to add branched sequencing to the
Xentu game engine. However it turns out Sutori has a lot of uses in other
situations too.

Thanks for checking out the project, and I hope you find it useful.

Kodaloid