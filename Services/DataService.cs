using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBlogProject.Data;
using TheBlogProject.Enums;
using TheBlogProject.Models;

namespace TheBlogProject.Services
{
    public class DataService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BlogUser> _userManager;

        public DataService(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<BlogUser> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task managedDataAsync()
        {
            //Create the DB from the Migrations
            await _dbContext.Database.MigrateAsync();
            //Task 1: Seeding a few roles into the system
            await SeedRolesAsync();

            //Task 2 :Seed a user into the system
            await SeedUsersAsync();
        }

        private async Task SeedRolesAsync()
        {
            //If there are already roles in the system, do nothing
            if (_dbContext.Roles.Any())
            {
                return;
            }

            //Otherwise we want to create a few roles
            foreach(var role in Enum.GetNames(typeof(BlogRole)))
            {
                //Need to use the Role manager to create roles
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private async Task SeedUsersAsync()
        {
            //If there are already Users in the system, do nothing.
            if (_dbContext.Users.Any())
            {
                return;
            }

            //Step 1: Creates A New Instance Of A BlogUser
            var adminUser = new BlogUser()
            {
                Email = "vicmo616@gmail.com",
                UserName = "vicmo616@gmail.com",
                FirstName = "Victor",
                LastName = "Ortiz",
                PhoneNumber = "(800) 555-1212",
                EmailConfirmed = true
            };

            //Step 2: Use the UserManager to Create a new user that is defined by adminUser
            await _userManager.CreateAsync(adminUser, "Abc&123!");

            //Step 3: Add this new user to the Administrator role
            await _userManager.AddToRoleAsync(adminUser, BlogRole.Administrator.ToString());


            // Step 1: repeat: Create the Moderator user
            var modUser = new BlogUser()
            {
                Email = "victormortiz616@outlook.com",
                UserName = "victormortiz616@outlook.com",
                FirstName = "Manuel",
                LastName = " Ortiz",
                PhoneNumber = "(800) 555-1213",
                EmailConfirmed= true
            };
            await _userManager.CreateAsync(modUser, "Abc&123!");
            await _userManager.AddToRoleAsync(modUser, BlogRole.Moderator.ToString());

        }




    }
}
