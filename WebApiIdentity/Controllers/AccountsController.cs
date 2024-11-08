﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiIdentity.Dtos;
using WebApiIdentity.Identity;

namespace WebApiIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public AccountsController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        [HttpGet]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
        {
            var user = new AppUser();
            user.Email = dto.Email;
            user.UserName = dto.UserName;

            var result = await _userManager.CreateAsync(user, dto.Password); // Save yapar.
            var role = new AppRole();
            role.Name = "Admin";
            await _roleManager.CreateAsync(role);

            //uSER ROLE ATAMA İŞLEMİŞ
            await _userManager.AddToRoleAsync(user,role.Name);
            await _userManager.AddClaimAsync(user, new Claim( "User", "Delete"));
            await _roleManager.AddClaimAsync(role, new Claim("Role", "List"));

            if (result.Succeeded)
            {
                return Created($"api/users/{Users().Id}", user);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> UsersById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            return Ok(user);
        }
    }
}
