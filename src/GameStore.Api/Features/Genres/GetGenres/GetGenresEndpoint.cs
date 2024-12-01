using GameStore.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Features.Genres.GetGenres
{
    public static class GetGenresEndpoint
    {
        public static void MapGetGenres(this IEndpointRouteBuilder app)
        {
            app.MapGet("/", (GameStoreContext dbContext) => dbContext.Genres
                .Select(g => new GenreDto(
                    g.Id,
                    g.Name
                ))
                .AsNoTracking());
        }
    }
}
