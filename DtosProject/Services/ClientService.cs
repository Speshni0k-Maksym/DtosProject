using DtosProject.DTOs;
using DtosProject.Models;
using DtosProject.Data;
using Microsoft.EntityFrameworkCore;

namespace DtosProject.Services
{
    public class ClientService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClientService> _logger;

        public ClientService(ApplicationDbContext context, ILogger<ClientService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddClientAsync(CreateClientDTO clientDto)
        {
            var newClient = new Client
            {
                FullName = clientDto.FullName,
                Email = clientDto.Email,
                Age = clientDto.Age,
                CreatedAt = DateTime.Now
            };

            _context.Clients.Add(newClient);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Client added: {FullName}", clientDto.FullName);
        }

        public async Task<List<ClientListItemDTO>> GetAllClientsAsync()
        {
            return await _context.Clients
                .Select(c => new ClientListItemDTO
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    Email = c.Email,
                    Age = c.Age
                })
                .ToListAsync();
        }

        public async Task<ClientDetailsDTO?> GetClientByIdAsync(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            if (client == null) return null;

            return new ClientDetailsDTO
            {
                Id = client.Id,
                FullName = client.FullName,
                Email = client.Email,
                Age = client.Age,
                CreatedAt = client.CreatedAt
            };
        }

        public async Task<bool> UpdateClientAsync(UpdateClientDTO clientDto)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == clientDto.Id);
            if (client == null) return false;

            client.FullName = clientDto.FullName;
            client.Email = clientDto.Email;
            client.Age = clientDto.Age;

            await _context.SaveChangesAsync();
            _logger.LogInformation("Client updated: {Id} - {FullName}", client.Id, client.FullName);
            return true;
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            if (client == null) return false;

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Client deleted: {Id}", id);
            return true;
        }

        public async Task<List<ClientListItemDTO>> SearchClientsAsync(SearchClientsDTO searchDto)
        {
            var query = _context.Clients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchDto.SearchTerm))
            {
                var term = searchDto.SearchTerm.ToLower();
                query = query.Where(c => c.FullName.ToLower().Contains(term) ||
                                         c.Email.ToLower().Contains(term));
            }

            if (searchDto.MinAge.HasValue)
            {
                query = query.Where(c => c.Age >= searchDto.MinAge.Value);
            }

            if (searchDto.MaxAge.HasValue)
            {
                query = query.Where(c => c.Age <= searchDto.MaxAge.Value);
            }

            return await query.Select(c => new ClientListItemDTO
            {
                Id = c.Id,
                FullName = c.FullName,
                Email = c.Email,
                Age = c.Age
            }).ToListAsync();
        }
    }
}