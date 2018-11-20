using EM.Elsukov.DB.Models;
using EM.Elsukov.DB.NHibernate.Interfaces;

namespace EM.Elsukov.DB.NHibernate
{
    public class NHUserRepository : NHBaseRepository<User>, IUserRepository
    {
        public override void Delete(User user)
        {
            if (user != null)
            {
                user.Status = (int)UserStatus.DELETED;
                Save(user);
            }
        }

        public User LoadByName(string name)
        {
            var session = NHibernateHelper.GetCurrentSession();

            var entity = session.QueryOver<User>()
                .And(u => u.Login == name)
                .SingleOrDefault();

            NHibernateHelper.CloseSession();

            return entity;
        }
    }
}