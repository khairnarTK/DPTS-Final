using DPTS.Logging.Models;

namespace DPTS.Logging.Contracts
{
    public interface ILogManager
    {
        void LogApplicationCalls(EventEntry eventLogEntry);
    }
}