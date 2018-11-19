using System;

namespace EM.Elsukov.DB.Models
{
    public class Note : IEntity
    {
        public virtual long Id { get; set; }

        public virtual string Title { get; set; }

        public virtual string Text { get; set; }

        public virtual string Tags { get; set; }

        public virtual DateTime CreateDtae { get; set; }

        public virtual long UserId { get; set; }

        public virtual NoteStatus Status { get; set; }
    }
    public enum NoteStatus
    {
        PUBLISHED,
        Draft,
        DELETED,
    }
}