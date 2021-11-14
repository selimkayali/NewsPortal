using System;
namespace NewsPortal.Models
{
    public class NewsDto
    {
        public NewsDto()
        {
        }

        public int NewsId { get; set; }
        public string NewsTitle { get; set; }
        public string NewsContent { get; set; }
    }
}

