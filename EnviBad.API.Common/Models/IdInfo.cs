namespace EnviBad.API.Common.Models
{
    /// <summary>
    /// Общая базовая сущность для всех моделей с идентификатором и датой создания
    /// </summary>
    public class IdInfo
    {
        public int Id { get; set; }
        public DateTimeOffset? CreationDateTime { get; set; }

        public IdInfo()
        {
            this.CreationDateTime = DateTimeOffset.Now;
        }
    }
}
