using AuthanticateAndAuthorizeFlow.Models;

namespace AuthanticateAndAuthorizeFlow.Services
{
    public class UserService
    {

        private List<User> _users = new List<User>()
        {
            new(){ Id=1, UserName="turkay", Password="123456", Role="admin", Email="turkay@test.com"},
            new(){ Id=2, UserName="emre", Password="123456", Role="editor", Email="emre@test.com"},
            new(){ Id=3, UserName="burcu", Password="123456", Role="client", Email="burcu@test.com"},

        };
        public User ValidateUser(string userName, string password)
        {
            return _users.SingleOrDefault(u => u.UserName == userName && u.Password == password);
        }
    }
}
