using GameStore.Api.Models;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";
List<Genre> genres =
[
    new Genre{
        Id = new Guid("20b80024-63e9-4925-94ec-4a51d522b825"),
        Name = "Fighting"
    },
    new Genre{
        Id = new Guid("20b80024-63e9-4925-94ec-4a51d522b826"),
        Name = "Kids and family"
    },
    new Genre{
        Id = new Guid("20b80024-63e9-4925-94ec-4a51d522b827"),
        Name = "Racing"
    },
    new Genre{
        Id = new Guid("20b80024-63e9-4925-94ec-4a51d522b828"),
        Name = "Roleplaying"
    },
    new Genre{
        Id = new Guid("20b80024-63e9-4925-94ec-4a51d522b829"),
        Name = "Sports"
    }
];
List<Game> games =
[
    new Game{
        Id = Guid.NewGuid(),
        Name = "Street Fighter II",
        Genre = genres[0],
        Price = 19.99m,
        ReleaseDate = new DateOnly(1992, 7, 15),
        Description = "test description"
    },
    new Game{
        Id = Guid.NewGuid(),
        Name = "DOTA 2",
        Genre = genres[3],
        Price = 4.99m,
        ReleaseDate = new DateOnly(2014, 10, 10),
        Description = "test description"
    },
    new Game{
        Id = Guid.NewGuid(),
        Name = "Vice City",
        Genre = genres[4],
        Price = 14.99m,
        ReleaseDate = new DateOnly(2004, 1, 20),
        Description = "test description"
    }
];

//GET /games
app.MapGet("/games", () => games.Select(g => new GameSummaryDto(
    g.Id,
    g.Name,
    g.Genre.Name,
    g.Price,
    g.ReleaseDate
    )));

//GET /games/9f8ebd12-77e5-4b6e-89af-a6a6b23ae7f1
app.MapGet("/games/{id}", (Guid id) =>
{
    Game? game = games.Find(g => g.Id == id);
    return game is null ? Results.NotFound() : Results.Ok(
        new GameDetailsDto(
            game.Id,
            game.Name,
            game.Genre.Id,
            game.Price,
            game.ReleaseDate,
            game.Description
            ));
})
.WithName(GetGameEndpointName);

//POST /games
app.MapPost("/games", (CreateGameDto gameDto) =>
{
    var genre = genres.Find(g => g.Id == gameDto.GenreId);

    if(genre is null)
    {
        return Results.BadRequest("Invalid genre Id");
    }

    var game = new Game {
        Id = Guid.NewGuid(),
        Name = gameDto.Name,
        Genre = genre,
        Price = gameDto.Price,
        ReleaseDate = gameDto.ReleaseDate,
        Description = gameDto.Description,
    };
    games.Add(game);
    return Results.CreatedAtRoute(GetGameEndpointName, 
        new { id = game.Id }, 
        new GameDetailsDto(game.Id, game.Name, game.Genre.Id, game.Price, game.ReleaseDate, game.Description));
})
.WithParameterValidation();

//PUT /games/9f8ebd12-77e5-4b6e-89af-a6a6b23ae7f1
app.MapPut("/games/{id}", (Guid id, UpdateGameDto gameDto) =>
{
    var existingGame = games.Find(g => g.Id == id);
    if(existingGame is null)
    {
        return Results.NotFound();
    }

    var genre = genres.Find(g => g.Id == gameDto.GenreId);

    if (genre is null)
    {
        return Results.BadRequest("Invalid genre Id");
    }

    existingGame.Name = gameDto.Name;
    existingGame.Genre = genre;
    existingGame.Price = gameDto.Price;
    existingGame.ReleaseDate = gameDto.ReleaseDate;
    existingGame.Description = gameDto.Description;
    return Results.NoContent();
})
.WithParameterValidation();

//DELETE /games/9f8ebd12-77e5-4b6e-89af-a6a6b23ae7f1
app.MapDelete("/games/{id}", (Guid id) =>
{
    games.RemoveAll(g => g.Id == id);
    return Results.NoContent();
});

//GET /genres
app.MapGet("/genres", () => genres.Select(g => new GenreDto(
        g.Id,
        g.Name
    )));

app.Run();

public record GameDetailsDto(
    Guid Id,
    string Name,
    Guid GenreId,
    decimal Price,
    DateOnly ReleaseDate,
    string Description);

public record GameSummaryDto(
    Guid Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
    );

public record CreateGameDto(
    [Required][StringLength(50)]string Name,
    Guid GenreId,
    [Range(1, 100)]decimal Price,
    DateOnly ReleaseDate,
    [Required][StringLength(500)]string Description);

public record UpdateGameDto(
    [Required][StringLength(50)] string Name,
    Guid GenreId,
    [Range(1, 100)] decimal Price,
    DateOnly ReleaseDate,
    [Required][StringLength(500)] string Description);

public record GenreDto(
    Guid Id, 
    string Name);