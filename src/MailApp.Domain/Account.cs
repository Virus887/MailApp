using System;

namespace MailApp.Domain
{
    public class Account : Entity<int>
    {
        public const int MaxNickLength = 100;

        public string Email { get; private set; }
        public string Nick { get; private set; }

        private Account()
        {
        }

        public Account(string nick, string email)
        {
            Nick = ValidateNick(nick);
            Email = ValidateEmail(email);
        }

        private string ValidateNick(string nick)
        {
            if (String.IsNullOrEmpty(nick))
            {
                throw new ArgumentNullException(nameof(nick));
            }

            if (nick.Length > MaxNickLength)
            {
                throw new ArgumentException("Nick is too long");
            }

            return nick;
        }

        private string ValidateEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            return email;
        }
    }
}