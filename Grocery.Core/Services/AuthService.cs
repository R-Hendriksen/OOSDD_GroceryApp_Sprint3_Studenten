using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IClientService _clientService;

        public AuthService(IClientService clientService)
        {
            _clientService = clientService;
        }

        public Client? Login(string email, string password)
        {
            Client? client = _clientService.Get(email);
            if (client == null) return null;
            if (PasswordHelper.VerifyPassword(password, client.Password)) return client;
            return null;
        }

        public Client? Register(string email, string password, string name = "Onbekend")
        {
            Client? client = _clientService.Get(email);
            if (!ValidateEmail(email)) return null;
            if (!ValidatePassword(password)) return null;
            string hashedPassword = PasswordHelper.HashPassword(password);
            return _clientService.Add(name!, email, hashedPassword);
        }

        /*
        private void ValidateName(string? name)
        {
        }
        */

        private bool ValidateEmail(string? email)
        {
            bool isValid = true;

            try { if (email == null) { isValid = false; } }
            catch { throw new ArgumentNullException("Geen e-mailadres ingevoerd."); }
            
            try { if (email!.Trim() == "") { isValid = false; } }
            catch { throw new ArgumentNullException("Geen e-mailadres ingevoerd."); }

            try { if (!email!.Contains('@')) { isValid = false; } }
            catch { throw new ArgumentException("Ongeldig e-mailadres."); }

            try { if (email.Contains(@"[ <>()\\/]")) { isValid = false; } }
            catch { throw new ArgumentException("Ongeldig e-mailadres."); }

            return isValid;
        }

        private bool ValidatePassword(string? password)
        {
            const int minPasswordLength = 4;
            const int maxPasswordLength = 24;
            bool isValid = true;

            try { if (password == null) { isValid = false; } }
            catch { throw new ArgumentNullException("Geen wachtwoord ingevoerd."); }

            try { if (password!.Trim() == "") { isValid = false; } }
            catch { throw new ArgumentNullException("Geen wachtwoord ingevoerd."); }

            try { if (password.Contains(@"[\\{}()/:<>|''"" ]")) { isValid = false; } }
            catch { throw new ArgumentException("Wachtwoord bevat één of meer illegale karakters."); }

            try { if (password.Length < minPasswordLength) { isValid = false; } }
            catch { throw new ArgumentException("Ingevoerd wachtwoord is te kort."); }

            try { if (password.Length > maxPasswordLength) { isValid = false; } }
            catch { throw new ArgumentException("Ingevoerd wachtwoord is te lang."); }

            return isValid;
        }
    }
}