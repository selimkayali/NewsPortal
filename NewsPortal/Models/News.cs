using System;
namespace NewsPortal.Models
{
    public class News
    {
        public News()
        {
        }
        public int NewsId { get; set; }
        public string NewsTitle { get; set; }
        public string NewsContent { get; set; }
        public bool IsActive { get; set; }
    }
}

