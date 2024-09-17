using Microsoft.AspNetCore.Components.Authorization;
using PortalMauiApp.Data;
using System.Security.Claims;

namespace PortalMauiApp.Services
{
    public interface IAccountService
    {
        bool LoginAsync(string email, string password);
        Task<bool> CheckAuthenticatedAsync();
        Task LogoutAsync();
        bool RegisterAsync(string email, string password);
    }
    public class AccountService : AuthenticationStateProvider, IAccountService
    {
        public AccountService()
        {
            _context = new DataContext();
        }
        private bool _authenticated = false;


        private readonly ClaimsPrincipal Unauthenticated =
           new(new ClaimsIdentity());
        private readonly DataContext _context;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            _authenticated = false;

            // default to not authenticated
            var user = Unauthenticated;

            try
            {
                string? Email = Preferences.Get("Email", null);

                if (string.IsNullOrEmpty(Email))
                {
                    return new AuthenticationState(user);
                }


                // in our system name and email are the same
                var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, Email),
                        new(ClaimTypes.Email, Email)
                    };

                // set the principal
                var id = new ClaimsIdentity(claims, nameof(AccountService));
                user = new ClaimsPrincipal(id);
                _authenticated = true;
            }
            catch { }
            await Task.Delay(0);
            // return the state
            return new AuthenticationState(user);
        }

        public bool LoginAsync(string email, string password)
        {
            // Check if the user exists in the database
            var user = _context.GetUserByEmail(email);

            if (user == null || !password.Equals(user.Password))
            {
                // Invalid email or password
                return false;
            }


            Preferences.Set("email", email);
            Preferences.Set("password", password);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

            return true;
        }

        public async Task<bool> CheckAuthenticatedAsync()
        {
            await GetAuthenticationStateAsync();
            return _authenticated;
        }

        public async Task LogoutAsync()
        {
            Preferences.Clear();
            await Task.Delay(0);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public bool RegisterAsync(string email, string password)
        {
            
            bool isRegistered = _context.RegisterUser(email, password, "Admin");

            return true;
        }
    }
}
