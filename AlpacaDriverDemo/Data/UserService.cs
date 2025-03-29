using ASCOM.Alpaca;

namespace AlpacaDriverDemo.Data
{
    internal class UserService : ASCOM.Alpaca.IUserService
	{
        public async Task<bool> Authenticate(string username, string password)
        {
            return await Task.Run(() =>
            {
                try
                {
                    return username == ServerSettings.UserName && Hash.Validate(ServerSettings.Password, password);
                }
                catch
                {
                    return false;
                }
            }

            );
        }

        public bool UseAuth
        {
            get => ServerSettings.UseAuth;
        }
    }
}