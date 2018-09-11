namespace Seats4Me.Data.Model
{
    public class Seat : DataWithId
    {
        public int Row { get; set; }
        public int Chair { get; set; }
    }
}
