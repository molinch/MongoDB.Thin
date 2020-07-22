# MongoDB.Thin
Thin layer over MongoDB driver for .NET. It is mostly extension methods and builders.
It allows to query MongoDB more easily.
The goal of this library is to stay simple, more complex cases can be built upon.

# Nuget
The nuget package name is `MongoDB.Thin`: https://www.nuget.org/packages/MongoDB.Thin/

# Usage

## ASP.NET Core
To add it to ASP.NET Core
```c#
services.AddMongo("YourConnectionString", "YourDatabaseName");
```

## Get a collection
Example where _database is an instance of IMongoDatabase. And Game is the collection holding the games.
```c#
_database.Collection<Game>();
```

## FindOne
Get a document from a collection by supplying a filtering c# expression
Example:
```c#
var game = await _games.FindOneAsync(g => g.Id == id);
```

## FindOneAndUpdate
Get a FindOneAndUpdate command by invoking `FindOneAndUpdate()` on your collection (IMongoCollection<TDocument>).
You can then filter it multiple times, and perform multiple modifications.
- Filters are defined by calling `Filter()` on the command, the method accepts a lambda that configures the filter builder: C# filtering expression, C# expression with MongoDB operators, or a JSON string.
- Modifications are defined by calling `Modify()` on the command, the method accepts a lambda that configures the modification builder. Normal MongoDB operators can be used.
- Options can be defined by calling `Options()` on the command, the method accepts a lambda that configures the options builder. Projections can be defined, targeting array fields can be defined using `WithArrayFilter`.
- Finally awaiting `ExecuteAsync` runs it asynchronously.

Example:
```c#
var command = _games.FindOneAndUpdate();
command.Filter(b => b.Match(g => g.GameType == gameType &&
		 !g.Players.Any(p => p.Id == player.Id) &&
		 g.Status == GameStatus.WaitingForPlayers));

if (duration.HasValue)
{
	command = command.Filter(b => b.Match(g => g.MaxDuration <= duration));
}

var game = await command.Update(b => b
	.Modify(g => g.Push(g => g.Players, player))
	.Modify(g => g.Inc(g => g.Version, 1)))
	.ExecuteAsync();
```

## UpdateOne
Get an UpdateOne command by invoking `UpdateOne()` on your collection (IMongoCollection<TDocument>).
The command is similar to FindOneAndUpdate, except that the options don't allow any projection (since no document is returned).

## Indexes
Indexes can easily be added using expressions.

Example:
```c#
var db = app.ApplicationServices.GetService<IMongoDatabase>()!;

db.Collection<Game>().Indexes
	.Add(g => g.GameType)
	.Add(g => g.Status)
	.Add(g => g.Players, p => p.Id);
```
Here Game collection got 3 indexes:
- GameType field ascending
- Status field ascending
- Players is an array, it got an index on Id field

## More usage examples
You can see more usage examples in OnlineBoardz repository:
- [GameRepository class](https://github.com/molinch/onlineboardz/blob/master/src/game-svc/Api/Persistence/GameRepository.cs)
- [TicTacToeRepository class](https://github.com/molinch/onlineboardz/blob/master/src/game-svc/Api/Persistence/TicTacToeRepository.cs)