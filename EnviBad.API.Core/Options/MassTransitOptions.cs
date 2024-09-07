namespace EnviBad.API.Core.Options
{
    /// <summary>
    /// Настройки подключения к MQ и работы MassTransit
    /// </summary>
    public class MassTransitOptions
    {
        public string RabbitUser { get; set; }
        public string RabbitPassword { get; set; }
        public string RabbitHost { get; set; }
    }
}
