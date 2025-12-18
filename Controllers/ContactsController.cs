using AddressBookApi.Data;
using AddressBookApi.DTOs;
using AddressBookApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpPost]
        public async Task<IActionResult> Add(ContactDto dto)
        {

            var exists = _context.Contacts.Any(c =>
                        c.UserId == UserId && c.Email == dto.Email);

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
            return Ok();
        }

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    return Ok(_context.Contacts.Where(c => c.UserId == UserId));
        //}

        #region Alternative GetAll using sorting and pagination
        [HttpGet]
        public IActionResult GetAll(
            string? sortBy,
            bool isDescending = false,
            int pageNumber = 1,
            int pageSize = 10)
        {
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
                _ => query.OrderBy(c => c.Id) // Default sorting
            };
            // Pagination
            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var contacts = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            var response = new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = contacts
            };
            return Ok(response);
        }
        #endregion



        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var contact = _context.Contacts
                .FirstOrDefault(c => c.Id == id && c.UserId == UserId);

            if (contact == null) return NotFound();
            return Ok(contact);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = _context.Contacts
                .FirstOrDefault(c => c.Id == id && c.UserId == UserId);

            if (contact == null) return NotFound();

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
