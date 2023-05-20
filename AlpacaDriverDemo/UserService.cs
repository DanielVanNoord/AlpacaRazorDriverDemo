using ASCOM.Alpaca;

namespace AlpacaDriverDemo
{
    internal class UserService : IUserService
    {
        public bool UseAuth => false;

        public async Task<bool> Authenticate(string username, string password)
        {
            return await Task.Run(() => true);
        }
    }
}