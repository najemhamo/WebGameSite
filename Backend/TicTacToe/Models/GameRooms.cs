namespace Models
{
    public class GameRoom
    {
        public Guid RoomId { get; set; }
        public int RoomCapacity { get; set; }
        public int CurrentPlayer { get; set; }
        public bool HaveWon { get; set; }
        public static List<GameRoom> GameRooms { get; } = new List<GameRoom>
        {
            new GameRoom { RoomId = Guid.NewGuid() },
            new GameRoom { RoomId = Guid.NewGuid() },
            new GameRoom { RoomId = Guid.NewGuid() }
        };

        public GameRoom()
        {
            RoomCapacity = 0;
            CurrentPlayer = 1;
            HaveWon = false;
        }

    }
}