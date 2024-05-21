using Microsoft.AspNetCore.Mvc;
using Repository;

namespace Endpoints
{
    public static class GameEndpoints
    {
        public static void ConfigureGameEndpoints(this WebApplication app)
        {
            var game = app.MapGroup("/tictactoe");
            game.MapGet("rooms", GetAllRooms);
            game.MapGet("rooms/{roomId}", GetRoom);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> GetAllRooms(IGameRepository gameRepository)
        {
            var rooms = await gameRepository.GetGameRooms();
            return Results.Ok(rooms);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> GetRoom(IGameRepository gameRepository, Guid roomId)
        {
            var room = await gameRepository.GetGameRoom(roomId);
            if (room == null)
            {
                return Results.BadRequest();
            }
            return Results.Ok(room);
        }
    }
}