using Models;

namespace Repository
{
    public interface IGameRepository
    {
        Task<IEnumerable<GameRoom>> GetGameRooms();
        Task<GameRoom> GetGameRoom(Guid roomId);
        Task<GameRoom> JoinGameRoom(Guid roomId);
        Task<GameRoom> LeaveGameRoom(Guid roomId);
    }
}