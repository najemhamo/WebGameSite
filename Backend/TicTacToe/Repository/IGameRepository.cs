using Models;

namespace Repository
{
    public interface IGameRepository
    {
        Task<IEnumerable<GameRoom>> GetGameRooms();
        Task<GameRoom> GetGameRoom(Guid roomId);
    }
}