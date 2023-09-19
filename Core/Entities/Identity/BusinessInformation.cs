namespace Core.Entities.Identity
{
    public class BusinessInformation
    {
        public int Id { get; set; }
        public string BusinessName { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime FoundedDate { get; set; }
        public string TaxIdentificationNumber { get; set; }
        public string Description { get; set; }
        public string WebsiteUrl { get; set; }

        public int AppUserId { get; set; }
        public virtual TenantUser AppUser { get; set; }

        public int PersonalInformationId { get; set; }
        public virtual PersonalInformation PersonalInformation { get; set; }

        public int AddressId { get; set; }
        public virtual Address Address { get; set; }
    }
}