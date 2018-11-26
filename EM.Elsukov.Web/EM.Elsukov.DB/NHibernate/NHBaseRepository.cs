using System;
using System.Collections.Generic;
using NHibernate;
using EM.Elsukov.DB.Repository;
using EM.Elsukov.DB.Models;

namespace EM.Elsukov.DB.NHibernate
{
    public class NHBaseRepository<T> : IEntityRepository<T> where T : class, IEntity
    {
        public virtual T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public virtual void Delete(T entity)
        {
            var session = NHibernateHelper.GetCurrentSession();

            try
            {
                using (var tx = session.BeginTransaction())
                {               
                    if (entity != null)
                    {
                        session.Delete(entity);
                        tx.Commit();
                    }
                }
            }
            finally
            {
                NHibernateHelper.CloseSession();
            }
        }

        public virtual IEnumerable<T> GetAll()
        {
            ISession session = NHibernateHelper.GetCurrentSession();

            var entities = session.CreateCriteria<T>().List<T>();

            NHibernateHelper.CloseSession();

            return entities;
        }

        public virtual T Load(long id)
        {
            var session = NHibernateHelper.GetCurrentSession();

            var entity = session.QueryOver<T>()
                .Where(u => u.Id == id)
                .SingleOrDefault();

            NHibernateHelper.CloseSession();

            return entity;
        }

        public virtual void Save(T entity)
        {
            ISession session = NHibernateHelper.GetCurrentSession();

            try
            {
                using (var tx = session.BeginTransaction())
                {
                    session.SaveOrUpdate(entity);
                    tx.Commit();
                }
            }
            finally
            {
                NHibernateHelper.CloseSession();
            }
        }
    }
}


