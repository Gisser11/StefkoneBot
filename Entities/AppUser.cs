using System.Collections.Generic;

namespace ValiBot.Entities
{
    public class AppUser : BaseEntity
    {
        public long ChatId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string LastCommand { get; set; }
        
        public int QuestionIndex { get; set; }

        public ICollection<RegisterForm> RegisterForms { get; set; }
    }
}