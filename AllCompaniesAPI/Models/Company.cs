using System.ComponentModel.DataAnnotations;

namespace AllCompaniesAPI.Models
{
    public class Company
    {
        [Key]
        public string Code { get; set; }
        public string? Url { get; set; }
        public string? CompanyName { get; set; }
        public string? City { get; set; }
        public string? IndependentAuditingFirm { get; set; }
        

    }
}
