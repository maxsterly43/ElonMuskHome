using System;
using EM.Elsukov.DB.Models;
using EM.Elsukov.DB.NHibernate.Interfaces;
using System.Collections.Generic;
using NHibernate.Criterion;
using System.Linq;

namespace EM.Elsukov.DB.NHibernate
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

        public IEnumerable<Note> LoadBySorted(string filed)
        {
            var session = NHibernateHelper.GetCurrentSession();

            var entities = session.CreateCriteria<Note>()
                .AddOrder(Order.Desc(filed))
                .List<Note>();

            NHibernateHelper.CloseSession();

            return entities;
        }

        public Note LoadByTitle(string title)
        {
            var session = NHibernateHelper.GetCurrentSession();

            var entity = session.QueryOver<Note>()
                .And(u => u.Title == title)
                .SingleOrDefault();

            NHibernateHelper.CloseSession();

            return entity;
        }

        public IEnumerable<Note> LoadByUserLogin(string login, string filedSort = "CreateDate")
        {
            var session = NHibernateHelper.GetCurrentSession();

            var user = session.QueryOver<User>()
                .And(u => u.Login == login)
                .SingleOrDefault();

            var entity = session.CreateCriteria<Note>()
                .Add(Restrictions.Eq("User", user)).AddOrder(Order.Desc(filedSort))
                .List<Note>();

            NHibernateHelper.CloseSession();

            return entity;
        }

        public IEnumerable<Note> LoadLike(string search, string sort)
        {
            var session = NHibernateHelper.GetCurrentSession();

            var notes = session.QueryOver<Note>()
                .Where(t => t.Title.IsLike(search, MatchMode.Anywhere)
                || t.Tags.IsLike(search, MatchMode.Anywhere)
                ).OrderBy(Projections.Property(sort)).Desc
                .List();


            NHibernateHelper.CloseSession();

            return notes;
        }

        public void SaveByProc(Note note)
        {
            throw new NotImplementedException();
        }
    }
}