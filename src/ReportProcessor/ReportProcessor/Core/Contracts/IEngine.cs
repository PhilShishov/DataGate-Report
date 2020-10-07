
namespace ReportProcessor.Core.Contracts
{
    using NLog;

    public interface IEngine
    {
        void Run(Logger logger);
    }
}
