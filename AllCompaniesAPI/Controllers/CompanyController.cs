using AllCompaniesAPI.Data;
using AllCompaniesAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DbExtensions;

namespace AllCompaniesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly DataContext _context;
            public CompanyController(DataContext context) 
            {
                _context = context;
            }
        /// <summary>
        /// You Can Check All Companies From Here.
        /// </summary>
        /// <returns>This endpoint returns a list of Companies With their data.</returns>
        [HttpGet]
        public async Task<ActionResult<List<Company>>> GetAllCompanies() 
        {
            var companies = await _context.Companies.ToListAsync();
            
            return Ok(companies);
        }
        /// <summary>
        /// You Can Check Companies From Here one by one with their Codes.
        /// </summary>
        /// <returns>this endpoint returns a Company with its data.</returns>
        [HttpGet("{Code}")]
        public async Task<ActionResult<Company>> GetCompany(string Code)
        {
            var company = await _context.Companies.FindAsync(Code);
            if(company == null)
            
                    return NotFound("Company Not Found.");
                
            return Ok(company); 


        }
        /// <summary>
        /// You Can Add a New Company From Here With Entering its Parameters.
        /// </summary>
        /// <remarks>
        /// All the parameterse except code in the request body can be null.
        ///  
        ///  NOTE: You can only add by one Company at a time.
        ///  
        /// Sample request:
        ///
        ///     POST /Company
        ///     {
        ///        "code":not null,
        ///        "url":null,
        ///        "companyName": null,
        ///        "city": null,
        ///        "independentAuditingFirm":null
        ///     }
        /// </remarks>
        /// <returns>this endpoint returns a list of Companies</returns>
        [HttpPost]
        public async Task<ActionResult<List<Company>>> AddCompany(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return Ok(await GetAllCompanies()); 
        }
        /// <summary>
        /// You Can Update a existing Company From Here with changing the Parameters.
        /// </summary>
        /// <returns>this endpoint returns a list of Companies</returns>
        [HttpPut]
        public async Task<ActionResult<List<Company>>> UpdateCompany(Company updatedCompany)
        {
            var dbCompany = await _context.Companies.FindAsync(updatedCompany.Code);
            if (dbCompany == null)

                return NotFound("Company Not Found.");

            dbCompany.Code = updatedCompany.Code;
            dbCompany.Url = updatedCompany.Url;
            dbCompany.CompanyName = updatedCompany.CompanyName;
            dbCompany.City = updatedCompany.City;
            dbCompany.IndependentAuditingFirm=updatedCompany.IndependentAuditingFirm;

            await _context.SaveChangesAsync();

            return Ok(await _context.Companies.ToListAsync());

        }
        /// <summary>
        /// You Can Delete a existing Company From Here with entering its Company Code.
        /// </summary>
        /// <returns>this endpoint returns a list of Companies</returns>
        [HttpDelete]
        public async Task<ActionResult<List<Company>>> DeleteCompany(string Code)
        {
            var dbCompany = await _context.Companies.FindAsync(Code);
            if (dbCompany == null)

                return NotFound("Company Not Found.");

            _context.Companies.Remove(dbCompany);
            await _context.SaveChangesAsync();

            return Ok(await _context.Companies.ToListAsync());

        }
      
    }
}
