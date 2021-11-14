using System;
using Microsoft.EntityFrameworkCore;

namespace NewsPortal.Models
{
    public class NewsDb : DbContext
    {
        public NewsDb(DbContextOptions<NewsDb> options) : base(options)
        {
        }

        public DbSet<News> News => Set<News>();
    }
}

