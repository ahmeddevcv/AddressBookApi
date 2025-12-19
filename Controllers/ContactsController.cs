using AddressBookApi.Data;
using AddressBookApi.DTOs;
using AddressBookApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AddressBookApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContactsController(AppDbContext context)
        {
            _context = context;
        }

        private int UserId =>
            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0;

        [HttpPost]
        public async Task<IActionResult> Add(ContactDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exists = await _context.Contacts
                .AnyAsync(c => c.UserId == UserId && c.Email == dto.Email);

            if (exists)
                return BadRequest("Contact with this email already exists.");

            var contact = new Contact
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                BirthDate = dto.BirthDate,
                UserId = UserId
            };

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Contact created", ContactId = contact.Id });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
                    string? sortBy,
                    bool isDescending = false,
                    int pageNumber = 1,
                    int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("Invalid pagination parameters");

            var query = _context.Contacts
                .Where(c => c.UserId == UserId);

            // Sorting
            query = sortBy?.ToLower() switch
            {
                "firstname" => isDescending
                    ? query.OrderByDescending(c => c.FirstName)
                    : query.OrderBy(c => c.FirstName),

                "lastname" => isDescending
                    ? query.OrderByDescending(c => c.LastName)
                    : query.OrderBy(c => c.LastName),

                "email" => isDescending
                    ? query.OrderByDescending(c => c.Email)
                    : query.OrderBy(c => c.Email),

                _ => isDescending
                    ? query.OrderByDescending(c => c.Id)
                    : query.OrderBy(c => c.Id)
            };


            // Pagination
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var contacts = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = contacts.Select(c => new ContactResponseDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                BirthDate = c.BirthDate
            });

            return Ok(new PagedResponse<ContactResponseDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = items
            });
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid contact id");

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == UserId);

            if (contact == null)
                return NotFound(new { Message = "Contact not found" });

            var response = new ContactResponseDto
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                PhoneNumber = contact.PhoneNumber,
                Email = contact.Email,
                BirthDate = contact.BirthDate
            };

            return Ok(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid contact id");

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == UserId);

            if (contact == null)
                return NotFound(new { Message = "Contact not found" });

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
