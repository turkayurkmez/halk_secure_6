using AuthFlowWithREST.Models;

namespace AuthFlowWithREST.Services
{
    public class UserService
    {
        private List<User> users = new List<User>()
        {
             new(){ Id=1, UserName="turkay", Password="123456", Role="admin"},
             new(){ Id=2, UserName="begum", Password="123456", Role="editor"},
             new(){ Id=1, UserName="ilkem", Password="123456", Role="client"},


        };

        public User ValidateUser(string userName, string password) => users.SingleOrDefault(u => u.UserName == userName && password == u.Password);
    }
}
