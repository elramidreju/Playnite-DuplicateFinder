# DuplicateFinder
DuplicateFinder is a [Playnite](https://playnite.link/) extension that offers a simple way to find duplicate games in your library.

In times of free Epic Games Store games, GOG giveaways, Itch.io collaborative bundles, and monthly Amazon Prime Gaming rewards, it's easier than ever to end up with a game library bloated with duplicates.

DuplicateFinder adds a sidebar view to your Playnite UI, allowing you to search for duplicate games and making it easier to find games you might want to hide or remove from your library.

## Features
- Identifies duplicate games in your library based on the game's name.
- Allows you to include or exclude hidden games from the search.
- Displays results in a detailed, ordered view showing the name, platform and hidden status of each game.
- Includes a '_Check for similarity_' option to search for **potentially** duplicate games with non-matching names.
- Offers a customizable _Tolerance_ setting to fine-tune the '_Check for similarity_' results.

## Contribute
If you feel DuplicateFinder is lacking an important feature or something needs fixing, please open an issue or email me about it before working on anything.

### Code Styling
If you want to contribute to the repository, please adhere to the following guidelines:

- Do not expose public fields.
- Private fields should use camelCase with preceding underscore.
- Public and private properties should use PascalCase.
- All methods (private and public) should use PascalCase.
- Indent with 4 spaces instead of tabs.
- Place the first curly brace `{` on a new line, not directly after the statement.
- Add an empty line between the end of a code block `}` and any additional expression.
- Always encapsulate the code body after `if`, `for`, `foreach`, `while`, etc. with curly braces.

Example:
```
if (true)
{
    DoSomething();
}

DoSomethingElse();
```
Instead of
```
if (true)
    DoSomething();
DoSomethingElse();
```

## Third-Party Libraries
This project uses the following third-party libraries:

- [Fastenshtein](https://github.com/DanHarltey/Fastenshtein) is licensed under the [MIT License](https://github.com/DanHarltey/Fastenshtein/blob/master/LICENSE) - Copyright (c) 2017 DanHartley
- [PlayniteSDK](https://playnite.link/) is licensed under the [MIT License](https://github.com/JosefNemec/Playnite/blob/master/LICENSE.md) - Copyright (c) 2020 Josef Nemec
