using Headstone.Framework.Models.Contexts;
using Headstone.Framework.Models.Logging;

namespace Headstone.Framework.Logging.Channels
{
    public class DbLogChannel : ILogChannel
    {
        public void Log(LogRecord record)
        {
            using (var db = new FrameworkDbContext())
            {
                // Add the record
                db.Logs.Add(record);

                // Save
                db.SaveChanges();
            }
        }
    }
}
