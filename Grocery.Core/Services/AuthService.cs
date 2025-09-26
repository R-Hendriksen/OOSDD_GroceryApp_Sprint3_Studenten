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
            ValidateEmail(email);
            ValidatePassword(password);
            string hashedPassword = PasswordHelper.HashPassword(password);
            return _clientService.Add(name!, email, hashedPassword);
        }

        /*
        private void ValidateName(string? name)
        {

        }
        */
        private void ValidateEmail(string? email)
        {
            bool isValid = true;
            try {
                if (email == null) { isValid = false; }
                    } 
            catch { 
                throw new ArgumentNullException("Geen wachtwoord ingevoerd."); }
            try
            {
                if (!email!.Contains('@')) { isValid = false; }
            }
            catch
            {
                throw new ArgumentException("Ongeldig e-mailadres."); }
                try
                {
                    if (email.Contains(' ')) { isValid = false; }
            }
                catch
                {
                    throw new ArgumentException("Ongeldig e-mailadres."); }
                    try
                    {
                        if (email.Contains(' ')) { isValid = false; }
                    }
                    catch
                    {
                        throw new ArgumentException("Ongeldig e-mailadres."); }

            return isValid;
        }

        private void ValidatePassword(string? password)
        {
            const int minPasswordLength = 8;
            const int maxPasswordLength = 24;

            if (password == null) throw new ArgumentNullException("Geen wachtwoord ingevoerd.");
            if (password.Trim() == "") throw new ArgumentNullException("Geen wachtwoord ingevoerd.");
            if (password.Contains(@"[\\{}()/:<>|''"" ]")) throw new ArgumentException("Wachtwoord bevat één of meer illegale karakters.");
            //password = Regex.Replace(password, @"[\\{}()/:<>|''""]", "");
            //if (password.Contains(" ")) throw new ArgumentNullException("Geen wachtwoord ingevoerd.");
            if (password.Length < minPasswordLength) throw new ArgumentException("Ingevoerd wachtwoord is te kort.");
            if (password.Length > maxPasswordLength) throw new ArgumentException("Ingevoerd wachtwoord is te lang.");
        }
    }
}
