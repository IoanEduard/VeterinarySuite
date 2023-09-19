namespace API.DTO
{
    public class SubscriptionPlanDto
    {
            public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Days { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}