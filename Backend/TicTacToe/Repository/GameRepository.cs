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

        public async Task<GameRoom> JoinGameRoom(Guid roomId)
        {
            var room = GameRoom.GameRooms.FirstOrDefault(x => x.RoomId == roomId);
            if (room == null)
            {
                return null;
            }

            if (room.RoomCapacity == 2)
            {
                return null;
            }

            room.RoomCapacity++;
            return room;
        }


        public async Task<GameRoom> LeaveGameRoom(Guid roomId)
        {
            var room = GameRoom.GameRooms.FirstOrDefault(x => x.RoomId == roomId);
            if (room == null)
            {
                return null;
            }

            if (room.RoomCapacity == 0)
            {
                return null;
            }

            room.RoomCapacity--;
            return room;
        }
    }
}