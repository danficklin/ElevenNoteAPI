using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElevenNote.Data;
using ElevenNote.Data.Entities;
using ElevenNote.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElevenNote.Services.User
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> RegisterUserAsync(UserRegister model)
        {
            if (await GetUserByEmailAsync(model.Email) != null || await GetUserByUsername(model.Username) != null)
            return false;
            var entity = new UserEntity
            {
                Email = model.Email,
                Username = model.Username,
                Password = model.Password,
                DateCreated = DateTime.Now
            };
            var passwordHasher = new PasswordHasher<UserEntity>();
            entity.Password = passwordHasher.HashPassword(entity, model.Password);
            _context.Users.Add(entity);
            var numberOfChanges = await _context.SaveChangesAsync();
            return numberOfChanges == 1;
        }
        private async Task<UserEntity> GetUserByEmailAsync(string email) // Get users by email helper method
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Email.ToLower() == email.ToLower());
        }
        private async Task<UserEntity> GetUserByUsername(string username) // Get users by email helper method
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Username.ToLower() == username.ToLower());
        }
        private async Task<UserDetail> GetUserByIdAsync(int userId)
        {
            var entity = await _context.Users.FindAsync(userId);
            if (entity is null)
                return null;
            var userDetail = new UserDetail
            {
                Id = entity.Id,
                Email = entity.Email,
                Username = entity.Username,
                Forename = entity.Forename,
                Surname = entity.Surname,
                DateCreated = entity.DateCreated
            };
            return userDetail;
        }
    }
}