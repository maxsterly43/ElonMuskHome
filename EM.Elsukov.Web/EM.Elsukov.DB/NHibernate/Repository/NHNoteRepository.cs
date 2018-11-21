using System;
using EM.Elsukov.DB.Models;
using EM.Elsukov.DB.NHibernate.Interfaces;
using System.Collections.Generic;
using NHibernate.Criterion;

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

            var entities = session.CreateCriteria<Note>().AddOrder(Order.Desc(filed)).List<Note>();

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

    }
}