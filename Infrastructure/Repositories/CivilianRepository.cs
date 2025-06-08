using Core.Models;
using Core.Repositories.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CivilianRepository : Repository<Civilian>, ICivilianRepository
    {
        private readonly AppDbContext _context;

        public CivilianRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Civilian?> GetByEmailAsync(string email)
        {
            return await _context.Civilians
                .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
        }

        public async Task<Civilian?> GetByNICAsync(string nic)
        {
            return await _context.Civilians
                .FirstOrDefaultAsync(c => c.NicNumber == nic);
        }

        public async Task<Civilian?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Civilians
                .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);
        }

    }
}
