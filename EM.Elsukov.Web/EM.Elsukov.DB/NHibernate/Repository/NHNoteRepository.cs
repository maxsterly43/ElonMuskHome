using EM.Elsukov.DB.Models;
using EM.Elsukov.DB.NHibernate.Interfaces;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Type;
using System;
using System.Collections.Generic;

namespace EM.Elsukov.DB.NHibernate.Repository
{
    public class NHNoteRepository : NHBaseRepository<Note>, INoteRepository
    {
        public override void Delete(Note note)
        {
            if (note != null)
            {
                note.Status = NoteStatus.DELETED;
                Save(note);
            }
        }

        public IEnumerable<Note> LoadBySorted(string sort)
        {
            var session = NHibernateHelper.GetCurrentSession();

            var entities = session.QueryOver<Note>()
                .Where(p => p.Status == NoteStatus.PUBLISHED)
                .OrderBy(Projections.Property(sort))
                .Desc.List();


            NHibernateHelper.CloseSession();

            return entities;
        }

        public IEnumerable<Note> LoadByUser(string login, string filedSort)
        {
            var session = NHibernateHelper.GetCurrentSession();

            var user = session.QueryOver<User>()
                 .Where(u => u.Login == login)
                 .SingleOrDefault();

            var notes = session.QueryOver<Note>()
                .Where(l => l.User == user)
                .Where(l => l.Status != NoteStatus.DELETED)
                .OrderBy(Projections.Property(filedSort)).Desc.List();

            NHibernateHelper.CloseSession();

            return notes;
        }
        public IEnumerable<Note> LoadLikeByUser(string login, string search, string sort)
        {
            var session = NHibernateHelper.GetCurrentSession();

            IEnumerable<Note> notes;

            var user = session.QueryOver<User>()
              .Where(u => u.Login == login)
              .SingleOrDefault();

            var query = session.QueryOver<Note>()
                .Where(l => l.User == user)
                .Where(l => l.Status != NoteStatus.DELETED)
                .Where(t => t.Title.IsLike(search, MatchMode.Start) || t.Tags.IsLike(search, MatchMode.Anywhere));

            if (sort == "CreateDate")
                notes = query.OrderBy(Projections.Property(sort)).Desc.List();
            else
                notes = query.OrderBy(Projections.Property(sort)).Asc.List();

            NHibernateHelper.CloseSession();

            return notes;
        }

        public IEnumerable<Note> LoadLike(string search, string sort)
        {
            var session = NHibernateHelper.GetCurrentSession();

            IEnumerable<Note> notes;

            var query = session.QueryOver<Note>()
               .Where(p => p.Status == NoteStatus.PUBLISHED);

            if (search != null)
                query.Where(t => t.Title.IsLike(search, MatchMode.Start) || t.Tags.IsLike(search, MatchMode.Anywhere));

            if (sort == "Login")
            {
                notes = query.JoinQueryOver<User>(c => c.User)
                    .OrderBy(m => m.Login)
                    .Asc.List();
            }
            else if (sort == "CreateDate")
                notes = query.OrderBy(Projections.Property(sort)).Desc.List();
            else
                notes = query.OrderBy(Projections.Property(sort)).Asc.List();
            return notes;
        }

        public bool SaveByProc(Note note)
        {

            if (note != null)
            {
                var session = NHibernateHelper.GetCurrentSession();

                IQuery query = session.CreateSQLQuery
                    ("exec CreateNote @Title=:Title, @Text=:Text,  @Tags=:Tags, @UserId=:UserId, @FileId=:FileId, @Status=:Status")
                      .SetString("Title", note.Title)
                      .SetString("Text", note.Text)
                      .SetString("Tags", note.Tags)
                      .SetInt64("UserId", note.User.Id)
                      .SetParameter("FileId", note.File?.Id ?? null)
                      .SetInt32("Status", (int)note.Status);

                var complite = query.ExecuteUpdate();

                NHibernateHelper.CloseSession();

                return Convert.ToBoolean(complite);
            }
            return false;
        }

        public void restoreNote(Note note)
        {
            if (note != null)
            {
                note.Status = NoteStatus.Draft;
                Save(note);
            }
        }
    }
}