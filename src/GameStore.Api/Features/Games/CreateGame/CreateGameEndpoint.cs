﻿using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Features.Games.GetGame;
using GameStore.Api.Models;

namespace GameStore.Api.Features.Games.PostGame
{
    public static class CreateGameEndpoint
    {
        public static void MapPostGame(this IEndpointRouteBuilder app, GameStoreData data)
        {
            app.MapPost("/", (CreateGameDto gameDto) =>
            {
                var genre = data.GetGenre(gameDto.GenreId);

                if (genre is null)
                {
                    return Results.BadRequest("Invalid genre Id");
                }

                var game = new Game
                {
                    Name = gameDto.Name,
                    Genre = genre,
                    Price = gameDto.Price,
                    ReleaseDate = gameDto.ReleaseDate,
                    Description = gameDto.Description,
                };
                data.AddGame(game);
                return Results.CreatedAtRoute(EndpointNames.GetGame,
                    new { id = game.Id },
                    new GameDetailsDto(game.Id, game.Name, game.Genre.Id, game.Price, game.ReleaseDate, game.Description));
            })
            .WithParameterValidation();
        }
    }
}