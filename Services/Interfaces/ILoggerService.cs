
namespace Crunchy.Services.Interfaces {

    public interface ILoggerService {
        void Info(string message);
        void Error(string message);
        void User(string message);
        void DevInfo(string message);
    }
}