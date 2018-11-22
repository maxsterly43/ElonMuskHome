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
                user.Status = UserStatus.DELETED;
                Save(user);
            }
        }

        public User LoadByLogin(string login)
        {
            var session = NHibernateHelper.GetCurrentSession();

            var entity = session.QueryOver<User>()
                .And(u => u.Login == login)
                .SingleOrDefault();

            NHibernateHelper.CloseSession();

            return entity;
        }
    }
}