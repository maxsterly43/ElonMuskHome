using System;

namespace EM.Elsukov.DB.Models
{
    public class User : IEntity
    {
        public virtual long Id { get; set; }

        public virtual string Login { get; set; }

        public virtual string Password { get; set; }

        public virtual string Name { get; set; }

        public virtual bool? Sex { get; set; }

        public virtual DateTime? BirthDay { get; set; }

        public virtual string Email { get; set; }

        public virtual int Status { get; set; }
    }

    public enum UserStatus
    {
        ACTIVE,
        DELETED
    }
}