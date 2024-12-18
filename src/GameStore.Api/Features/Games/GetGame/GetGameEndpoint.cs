﻿using GameStore.Api.Data;
using GameStore.Api.Models;
using GameStore.Api.Features.Games.Constants;

namespace GameStore.Api.Features.Games.GetGame
{
    public static class GetGameEndpoint
    {
        //GET /games/9f8ebd12-77e5-4b6e-89af-a6a6b23ae7f1
        public static void MapGetGame(this IEndpointRouteBuilder app)
        {
            app.MapGet("/{id}", (
                Guid id, 
                GameStoreContext dbContext) =>
            {
                Game? game = dbContext.Games.Find(id);
                return game is null ? Results.NotFound() : Results.Ok(
                    new GameDetailsDto(
                        game.Id,
                        game.Name,
                        game.GenreId,
                        game.Price,
                        game.ReleaseDate,
                        game.Description
                        ));
            })
            .WithName(EndpointNames.GetGame);
        }
    }
}
