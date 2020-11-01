using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailApp.Domain
{
    public class Account : Entity<int>
    {

        public string Nick { get; set; }
        public const int MaxNickLength = 100;

        public Account(string nick)
        {

            ValidateNick(nick);
            this.Nick = nick;

        }

        private void ValidateNick(string nick)
        {
            if (String.IsNullOrEmpty(nick))
            {
                throw new ArgumentNullException(nameof(nick));
            }
            if (nick.Length > MaxNickLength)
            {
                throw new ArgumentException("Nick is too long");

            }
        }
    }
}
