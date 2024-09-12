namespace EnviBad.API.Common.Models.Options
{
    /// <summary>
    /// Настройки подключения к MQ и работы MassTransit
    /// </summary>
    public class MassTransitOptions
    {
        public string? RabbitUser { get; set; }
        public string? RabbitPassword { get; set; }
        public string? RabbitHost { get; set; }
        public ushort? RabbitPort { get; set; }
    }
}
