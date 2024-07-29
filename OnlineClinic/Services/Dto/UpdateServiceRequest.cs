namespace OnlineClinic.Services.Dto
{
    public class UpdateServiceRequest
    {
        public string? Name { get; set; }

        public string? Descriptions { get; set; }

        public int? Time { get; set; }

        public double? Price { get; set; }
    }
}
