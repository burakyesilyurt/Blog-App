﻿namespace BlogDAL.Models
{
    public class ViewCount
    {
        public int ViewCountId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}