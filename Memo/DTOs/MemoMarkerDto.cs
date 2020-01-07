using System;
using Memo.Models;

namespace Memo.DTOs
{
    public class MemoMarkerDto
    {
        public long Id { get; set; }
        public Coordinates Coordinates { get;  set; }
        public DateTime Created { get;  set; }
        public DateTime Updated { get;  set; }
        public string Name { get;  set; }
        public string Description { get;  set; }
        public string UserName { get; set; }
        public bool IsCenterPoint { get; set; }
    }
}