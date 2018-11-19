using EM.Elsukov.DB.Models;

namespace EM.Elsukov.DB.NHibernate.Interfaces
{
    public interface IUserRepository
    {
        User LoadByName(string name);
    }
}
