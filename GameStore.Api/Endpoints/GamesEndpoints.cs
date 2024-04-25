using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints
{
    public static class GamesEndpoints
    {
        const string GetGameEndpointName = "GetGame";

        private static readonly List<GameDto> games = [
            new(
                1,
                "Street Fighter II",
                "Fighting",
                19.99M,
                new DateOnly(1992, 7, 15)
            ),
            new(
                2,
                "Final Fantasy XIV",
                "Roleplaying",
                59.99M,
                new DateOnly(2010, 9, 30)
            ),
            new (
                3,
                "FIFA 23",
                "Sports",
                69.99M,
                new DateOnly(2022, 9, 27)
            )
        ];

        public static WebApplication MapGamesEndPoints(this WebApplication app){
            app.MapGet("games", () => games);

            app.MapGet("games/{id}", (int id) => {
                GameDto? game = games.Find(game => game.Id == id);

                return game is null ? Results.NotFound() : Results.Ok(game);
            })
            .WithName(GetGameEndpointName);

            app.MapPost("games", (CreateGameDto newGame) => {
                GameDto game = new(
                    games.Count + 1,
                    newGame.Name,
                    newGame.Genre,
                    newGame.Price,
                    newGame.ReleaseDate
                );
                games.Add(game);

                return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
            });

            app.MapPut("games/{id}", (int id, UpdateGameDto updatedGame) => {

                var index = games.FindIndex(game => game.Id == id);
                if (index == -1) {
                    return Results.NotFound();
                }

                games[index] = new GameDto(
                    id,
                    updatedGame.Name,
                    updatedGame.Genre,
                    updatedGame.Price,
                    updatedGame.ReleaseDate
                );

                return Results.NoContent();
            });

            app.MapDelete("games/{id}", (int id) => {
                games.RemoveAll(game => game.Id == id);

                return Results.NoContent();
            });

            return app;
        }
    }
}