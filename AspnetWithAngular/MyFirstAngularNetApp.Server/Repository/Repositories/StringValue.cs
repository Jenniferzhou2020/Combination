namespace MyFirstAngularNetApp.Server.Repository.Repositories
{
    public static class StringValue
    {
        public const string Succeed = "Succeed";
        public const string Failed = "Failed";
        public const string Started = " Started";
        public const string Completed = " Completed";
    }
    public static class LogSteps
    {
        public const string BeforeUpdate = "BeforeUpdate";
        public const string AfterUpdate = "AfterUpdate";
        public const string Insert = "Insert";
    }
    public static class RecordStatus
    {
        public const int Active = 1;
        public const int Inactive = 0;
    }
}
