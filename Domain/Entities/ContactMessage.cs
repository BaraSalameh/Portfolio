﻿namespace Domain.Entities
{
    public class ContactMessage : AbstractEntity
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public Guid UserID { get; set; }
        public User User { get; set; }
    }
}
