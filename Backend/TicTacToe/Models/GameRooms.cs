namespace Models
{
    public class GameRoom
    {
        public Guid RoomId { get; set; }
        public int RoomCapacity { get; set; }
        public string PlayerX { get; set; }
        public string PlayerO { get; set; }
        public int[] Board { get; set; } = new int[9]; // TicTacToe board 3x3

        public static List<GameRoom> GameRooms { get; } = new List<GameRoom>
        {
            new GameRoom { RoomId = Guid.NewGuid() },
            new GameRoom { RoomId = Guid.NewGuid() },
            new GameRoom { RoomId = Guid.NewGuid() }
        };

        public GameRoom()
        {
            RoomCapacity = 0;
        }
    }
}
