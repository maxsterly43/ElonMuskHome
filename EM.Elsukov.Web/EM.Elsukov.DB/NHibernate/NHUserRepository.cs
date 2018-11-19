using EM.Elsukov.DB.Models;

namespace EM.Elsukov.DB.NHibernate
{
    public class NHUserRepository : NHBaseRepository<User>
    {
        public override void Delete(User user)
        {
            if (user != null)
            {
                user.Status = UserStatus.DELETED;
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