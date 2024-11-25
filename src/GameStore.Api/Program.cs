using GameStore.Api.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<Game> games =
[
    new Game{
        Id = Guid.NewGuid(),
        Name = "Street Fighter II",
        Genre = "Fighting",
        Price = 19.99m,
        ReleaseDate = new DateOnly(1992, 7, 15)
    },
    new Game{
        Id = Guid.NewGuid(),
        Name = "DOTA 2",
        Genre = "Moba",
        Price = 4.99m,
        ReleaseDate = new DateOnly(2014, 10, 10)
    },
    new Game{
        Id = Guid.NewGuid(),
        Name = "Vice City",
        Genre = "Person",
        Price = 14.99m,
        ReleaseDate = new DateOnly(2004, 1, 20)
    }
];

//GET /games
app.MapGet("/games", () => games);

//GET /games/9f8ebd12-77e5-4b6e-89af-a6a6b23ae7f1
app.MapGet("/games/{id}", (Guid id) =>
{
    Game? game = games.Find(g => g.Id == id);
    return game is null ? Results.NotFound() : Results.Ok(game);
})
.WithName(GetGameEndpointName);

//POST /games
app.MapPost("/games", (Game game) =>
{
    if (string.IsNullOrEmpty(game.Name))
    {
        return Results.BadRequest("Name is required");
    }

    game.Id = Guid.NewGuid();
    games.Add(game);
    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
})
.WithParameterValidation();

//PUT /games/9f8ebd12-77e5-4b6e-89af-a6a6b23ae7f1
app.MapPut("/games/{id}", (Guid id, Game game) =>
{
    var existingGame = games.Find(g => g.Id == id);
    if(existingGame is null)
    {
        return Results.NotFound();
    }
    existingGame.Name = game.Name;
    existingGame.Genre = game.Genre;
    existingGame.Price = game.Price;
    existingGame.ReleaseDate = game.ReleaseDate;
    return Results.NoContent();
})
.WithParameterValidation();

//DELETE /games/9f8ebd12-77e5-4b6e-89af-a6a6b23ae7f1
app.MapDelete("/games/{id}", (Guid id) =>
{
    games.RemoveAll(g => g.Id == id);
    return Results.NoContent();
});

app.Run();
