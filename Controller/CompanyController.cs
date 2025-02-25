using CompanyAPI.Data;
using CompanyAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly CompanyDbContext _context;

    public CompanyController(CompanyDbContext context)
    {
        _context = context;
    }

    // GET: api/Company
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Company>>> GetAll()
    {
        return await _context.Companies.ToListAsync();
    }

    // GET: api/Company/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Company>> GetByID(int id)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company == null)
        {
            return NotFound();
        }
        return company;
    }

    // POST: api/Company
    [HttpPost]
    public async Task<ActionResult<Company>> Post(Company company)
    {
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetByID), new { id = company.ID }, company);
    }


    // DELETE: api/Company/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company == null)
        {
            return NotFound();
        }

        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // PUT: api/Company/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCompany(int id, Company company)
    {
        if (id != company.ID)
        {
            return BadRequest("Company ID mismatch");
        }

        var existingCompany = await _context.Companies.FindAsync(id);
        if (existingCompany == null)
        {
            return NotFound();
        }

        //Update only the fields that changed
        existingCompany.Name = company.Name;
        existingCompany.No_Of_Emp = company.No_Of_Emp;
        existingCompany.Location = company.Location;
        existingCompany.Type = company.Type;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(500, "An error occurred while updating the company.");
        }

        return NoContent();
    }

}