using GameStore.Api.Models;

namespace GameStore.Api.Data
{
    public class GameStoreData
    {
        private readonly List<Genre> genres =
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

        private readonly List<Game> games;

        public GameStoreData()
        {
            games = 
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
        }

        public IEnumerable<Game> GetGames() => games;

        public Game? GetGame(Guid id) => games.Find(g => g.Id == id);

        public void AddGame(Game game)
        {
            game.Id = Guid.NewGuid();
            games.Add(game);
        }

        public void RemoveGame(Guid id)
        {
            games.RemoveAll(g => g.Id == id);
        }

        public IEnumerable<Genre> GetGenres() => genres;
        public Genre? GetGenre(Guid id) => genres.Find(genre => genre.Id == id);
    }
}
