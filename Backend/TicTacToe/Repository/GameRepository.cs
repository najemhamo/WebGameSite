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

    }
}