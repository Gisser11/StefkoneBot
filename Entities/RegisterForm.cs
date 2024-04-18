namespace ValiBot.Entities
{
    public class RegisterForm : BaseEntity
    {
        public string NeededPlatform { get; set; }

        public long CurrentVolume { get; set; }
        
        public long AppUserId { get; set; }

        public AppUser AppUser { get; set; }

    }
}