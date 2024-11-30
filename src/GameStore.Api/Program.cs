using GameStore.Api.Data;
using GameStore.Api.Models;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

GameStoreData data = new();

//GET /games
app.MapGet("/games", () => data.GetGames().Select(g => new GameSummaryDto(
    g.Id,
    g.Name,
    g.Genre.Name,
    g.Price,
    g.ReleaseDate
    )));

//GET /games/9f8ebd12-77e5-4b6e-89af-a6a6b23ae7f1
app.MapGet("/games/{id}", (Guid id) =>
{
    Game? game = data.GetGame(id);
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
    var genre = data.GetGenre(gameDto.GenreId);

    if(genre is null)
    {
        return Results.BadRequest("Invalid genre Id");
    }

    var game = new Game {
        Name = gameDto.Name,
        Genre = genre,
        Price = gameDto.Price,
        ReleaseDate = gameDto.ReleaseDate,
        Description = gameDto.Description,
    };
    data.AddGame(game);
    return Results.CreatedAtRoute(GetGameEndpointName, 
        new { id = game.Id }, 
        new GameDetailsDto(game.Id, game.Name, game.Genre.Id, game.Price, game.ReleaseDate, game.Description));
})
.WithParameterValidation();

//PUT /games/9f8ebd12-77e5-4b6e-89af-a6a6b23ae7f1
app.MapPut("/games/{id}", (Guid id, UpdateGameDto gameDto) =>
{
    var existingGame = data.GetGame(id);
    if(existingGame is null)
    {
        return Results.NotFound();
    }

    var genre = data.GetGenre(gameDto.GenreId);

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
    data.RemoveGame(id);
    return Results.NoContent();
});

//GET /genres
app.MapGet("/genres", () => data.GetGenres().Select(g => new GenreDto(
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