namespace Memo.Models
{
    public class MemoMarkerRequest
    {
        public string Name { get; set; }
        public string Description { get;  set; }
        public Coordinates Coordinates { get; set; }
    }
}