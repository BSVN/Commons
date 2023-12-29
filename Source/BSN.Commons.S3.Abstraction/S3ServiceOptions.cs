namespace BSN.Commons.S3.Abstraction
{
    public class S3ServiceOptions
    {
        public string AccessKey { get; set; }

        public string EndPoint { get; set; }

        public string SecretKey { get; set; }

        public int TempTokenLifeTime { get; set; } = int.MaxValue;
    }
}