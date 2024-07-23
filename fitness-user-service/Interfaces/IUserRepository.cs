using fitness_db.Models;
using fitness_user_service.Dto.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fitness_db.Interfaces
{
    public interface IUserRepository 
    {
        public bool CreateUser(User User);
        public User UpdateUser(User User);
        public bool DeleteUser(User User);
        public ICollection<User> GetUsers();
        public User GetUser(int userId);
    }
}
