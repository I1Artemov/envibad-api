namespace EnviBad.API.Common
{
    public class Const
    {
        public const string AppVersion = "1.0.0.0";
        public const double MinReportAreRadius = 10;
        public const double MaxReportAreRadius = 500;
    }

    public enum ReportStatus
    {
        Created = 0,
        InProgress = 1,
        Completed = 2,
        Failed = 3
    }
}
