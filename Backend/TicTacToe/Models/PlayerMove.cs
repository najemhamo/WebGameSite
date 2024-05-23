namespace Models
{
    public class PlayerMove
    {
        public Guid RoomId { get; set; }
        public int[] Board { get; set; }
        public GameState GameState { get; set; }
        public string Player { get; set; }
    }
}