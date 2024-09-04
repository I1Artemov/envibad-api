namespace EnviBad.API.Common.Log
{
    /// <summary>
    /// Интерфейс логгера
    /// </summary>
    public interface IAppLogger
    {
        void Error(Exception ex);
        void Error(Exception ex, string message);
        void Info(string message);
        void Warn(string message);
    }
}
