namespace Models
{
    public class GameRoom
    {
        public int Id { get; set; }
        public Guid RoomId { get; set; }
        public int RoomCapacity { get; set; }
        public string Difficulty { get; set; } // Easy or Hard
        public string PlayerX { get; set; }
        public string PlayerO { get; set; }
        public int[] Score { get; set; } = new int[2];
        public int[] Board { get; set; } = new int[9];
        public string Winner { get; set; }

        public static List<GameRoom> GameRooms { get; } = new List<GameRoom>
        {
            new GameRoom { RoomId = Guid.NewGuid(), Id = 1 },
            new GameRoom { RoomId = Guid.NewGuid(), Id = 2},
            new GameRoom { RoomId = Guid.NewGuid(), Id = 3}
        };

        public GameRoom()
        {
            RoomCapacity = 0;
        }
    }
}
