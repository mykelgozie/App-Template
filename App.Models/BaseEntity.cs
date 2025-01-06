using System;

namespace App.Models
{
    public abstract class BaseEntity
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string UserId { get; set; }

        public BaseEntity()
        {
            CreatedAt = DateTime.UtcNow.AddHours(1);
            ModifiedAt = DateTime.UtcNow.AddHours(1);
            Id = Guid.NewGuid().ToString();
            IsDeleted = false;
        }
    }
}
