using Models;

namespace Repository
{
    public interface IGameRepository
    {
        Task<IEnumerable<GameRoom>> GetGameRooms();
        Task<GameRoom> GetGameRoom(Guid roomId);
        Task CreateSinglePlayerRoom(string playerName, string difficulty);
        Task DeleteGameRoom(Guid roomId);
    }
}