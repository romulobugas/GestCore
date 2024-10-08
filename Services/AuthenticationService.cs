﻿using GestCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace GestCore.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<UserApp> RegisterUserAsync(string name, string password)
        {
            var existingUser = await _context.UserApps.FirstOrDefaultAsync(u => u.Name == name);
            if (existingUser != null)
            {
                throw new Exception("Usuário já existe.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new UserApp
            {
                Name = name,
                Password = hashedPassword
            };

            _context.UserApps.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<UserApp> AuthenticateUserAsync(string name, string password)
        {
            var user = await _context.UserApps.FirstOrDefaultAsync(u => u.Name == name);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return user;
            }
            return null;
        }
    }
}
