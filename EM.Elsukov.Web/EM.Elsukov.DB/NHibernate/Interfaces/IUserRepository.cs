using EM.Elsukov.DB.Models;

namespace EM.Elsukov.DB.NHibernate.Interfaces
{
    public interface IUserRepository
    {
        User LoadByLogin(string login);
    }
}
