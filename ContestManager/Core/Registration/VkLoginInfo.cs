namespace Core.Registration
{
    public class VkLoginInfo
    {
        public long Expire { get; set; }
        public string Mid { get; set; }
        public string Secret { get; set; }
        public string Sid { get; set; }
        public string Sig { get; set; }
    }
}