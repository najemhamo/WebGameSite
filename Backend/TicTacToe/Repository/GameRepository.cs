using Microsoft.AspNetCore.Http.HttpResults;
using Models;

namespace Repository
{
    public class GameRepository : IGameRepository
    {
        public async Task<IEnumerable<GameRoom>> GetGameRooms()
        {
            return await Task.FromResult(GameRoom.GameRooms);
        }

        public async Task<GameRoom> GetGameRoom(Guid roomId)
        {
            return await Task.FromResult(GameRoom.GameRooms.FirstOrDefault(x => x.RoomId == roomId));
        }

        public async Task CreateSinglePlayerRoom(string playerName, string difficulty)
        {
            var newRoom = new GameRoom
            {
                RoomId = Guid.NewGuid(),
                RoomCapacity = 1,
                PlayerX = playerName,
                Difficulty = difficulty
            };

            GameRoom.GameRooms.Add(newRoom);
            await Task.FromResult(GameRoom.GameRooms);
        }

        public async Task DeleteGameRoom(Guid roomId)
        {
            var room = GameRoom.GameRooms.FirstOrDefault(x => x.RoomId == roomId);
            if (room != null)
            {
                GameRoom.GameRooms.Remove(room);
            }
            await Task.FromResult(GameRoom.GameRooms);
        }
    }
}