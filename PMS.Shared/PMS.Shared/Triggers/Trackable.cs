using EntityFrameworkCore.Triggers;

namespace PMS.Shared.Triggers
{
    public abstract class Trackable
    {
        static Trackable()
        {
            var currentTime = DateTimeOffset.Now;

            Triggers<Trackable>.Inserting += entry =>
            {
                entry.Entity.CreatedAt = currentTime;
                entry.Entity.UpdatedAt = currentTime;
            };
            Triggers<Trackable>.Updating += entry =>
            {
                entry.Entity.UpdatedAt = currentTime;
            };
        }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}