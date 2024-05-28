using Models;

namespace Repository
{
    public interface IGameRepository
    {
        Task<IEnumerable<GameRoom>> GetGameRooms();
        Task<GameRoom> GetGameRoom(Guid roomId);
        Task CreateGameRoom(GameRoom gameRoom);
        Task DeleteGameRoom(Guid roomId);
    }
}