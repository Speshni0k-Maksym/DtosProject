using DtosProject.DTOs;
using DtosProject.Models;

namespace DtosProject.Services
{
    public class ClientService
    {
        private readonly List<Client> _clients = new List<Client>();
        public void AddClient(CreateClientDTO clientDto)
        {
            var newClient = new Client
            {
                Id = _clients.Count + 1,
                FullName = clientDto.FullName,
                Email = clientDto.Email,
                Age = clientDto.Age,
                CreatedAt = DateTime.Now
            };
            _clients.Add(newClient);
        }
        public List<ClientListItemDTO> GetAllClients()
        {
            return _clients.Select(c => new ClientListItemDTO
            {
                Id = c.Id,
                FullName = c.FullName,
                Email = c.Email,
                Age = c.Age
            }).ToList();
        }
        public ClientDetailsDTO? GetClientById(int id)
        {
            var client = _clients.FirstOrDefault(c => c.Id == id);
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
        public bool UpdateClient(UpdateClientDTO clientDto)
        {
            var client = _clients.FirstOrDefault(c => c.Id == clientDto.Id);
            if (client == null) return false;

            client.FullName = clientDto.FullName;
            client.Email = clientDto.Email;
            client.Age = clientDto.Age;

            return true;
        }
        public bool DeleteClient(int id)
        {
            var client = _clients.FirstOrDefault(c => c.Id == id);
            if (client == null) return false;

            return _clients.Remove(client);
        }
        public List<ClientListItemDTO> SearchClients(SearchClientsDTO searchDto)
        {
            var query = _clients.AsQueryable();

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

            return query.Select(c => new ClientListItemDTO
            {
                Id = c.Id,
                FullName = c.FullName,
                Email = c.Email,
                Age = c.Age
            }).ToList();
        }
    }
}