using Clinics.Core.Interfaces;
using Clinics.Core.Models;
using Clinics.Core.Models.Authentication;
using Clinics.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Clinics.EF.Repositories
{
    public class AuthRepository : IAuth
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly IConfiguration _configuration;
        protected ClinicContext _context;

        public AuthRepository(UserManager<ApplicationUser> userManager, IConfiguration configuration, ClinicContext context)
        {
            _userManger = userManager;
            _configuration = configuration;
            _context = context;

        }



        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (await _userManger.FindByEmailAsync(model.Email) is not null)
                return new AuthModel { Message = "Email is already registered!" };

            if (await _userManger.Users.AnyAsync(u => u.FirstName == model.FirstName && u.LastName == model.LastName))
            {
                return new AuthModel { Message = "Username is already registered!" };
            }


            var user = new ApplicationUser
            {

                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.FirstName+model.LastName,
            };

            var result = await _userManger.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new AuthModel { Message = errors };
            }


            // Save the changes to the database
            await _context.SaveChangesAsync();

            //***************** Determine the role based on the account type
            string role = "";
            switch (model.AccountType)
            {
                case 1:
                    role = "Student";
                    break;
                case 2:
                    role = "Parent";
                    break;
                case 3:
                    role = "Teacher";
                    break;
                case 4:
                    role = "MedicalSupervisor";
                    break;
                case 5:
                    role = "FinancialSupervisor";
                    break;
                case 6:
                    role = "SocialSupervisor";
                    break;
            }

            // Add the user to the role
            await _userManger.AddToRoleAsync(user, role);

            /**************ENDDDDDD ****/


            ////////////making a new account 
            var account = await NewAccount(model);

            if (account == null)
            {
                await _userManger.DeleteAsync(user);
                await _context.SaveChangesAsync();
                return new AuthModel { Message = "Failed to create account." };
            }

            /////////////end
            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthModel
            {
                Email = user.Email,
                UserId = user.Id,
                Expiration = DateTime.UtcNow.AddDays(7),
                IsAuthenticated = true,
                Roles = new List<string> { role },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
        public async Task<AuthModel> LoginAsync(Login model)
        {
            var authModel = new AuthModel();

            // var user = await _userManger.Users.FirstOrDefaultAsync(x => x.UserName == model.Username);
            var user = await _userManger.FindByEmailAsync(model.Email);

            if (user == null || !await _userManger.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect";
                return authModel;
            }
            var rolesList = await _userManger.GetRolesAsync(user);
            authModel.Roles = rolesList.ToList();

            var jwtSecurityToken = await CreateJwtToken(user);
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.FirstName = user.FirstName;
            authModel.LastName = user.LastName;
            authModel.Expiration = DateTime.UtcNow.AddDays(7);
            authModel.UserId = user.Id;

            return authModel;
        }
        //Create  JWT Token XDD 
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManger.GetClaimsAsync(user);
            var roles = await _userManger.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public Task<AuthModel> ConfirmEmailAsync(string userid, string token)
        {
            throw new NotImplementedException();
        }

        public Task<AuthModel> ForgetPasswordAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthModel> ResetPasswordAsync(ResetPassword model)
        {
            // Get the user by email
            var user = await _userManger.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new AuthModel { Message = "User not found." };
            }

            // Reset the password without using a token
            

            var newPasswordHash = _userManger.PasswordHasher.HashPassword(user, model.NewPassword);
            user.PasswordHash = newPasswordHash;

            var result = await _userManger.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new AuthModel { Message = errors };
            }

            return new AuthModel { Message = "Password reset successful." };
        }


        public enum AccountType
        {
            Student,
            Parent,
            Teacher,
            MedicalSupervisor,
            FinancialSupervisor,
            SocialSupervisor
        }

        public async Task<object> NewAccount(RegisterModel model)
        {
            var user = await _userManger.FindByEmailAsync(model.Email);
            if (user != null)
            {
                switch (model.AccountType)
                {
                    case 1:

                        var student = new Student
                        {
                            UserId = user.Id,
                            DateofBirth = model.DateofBirth,
                            gender = model.gender,
                            address = model.address
                        };
                        _context.Students.Add(student);
                        await _context.SaveChangesAsync();
                        return student;

                    case 2:
                        var parent = new Parent
                        {
                            UserId = user.Id,                           
                            gender = model.gender,                           
                            address = model.address
                        };
                        _context.Parents.Add(parent);
                        await _context.SaveChangesAsync();
                        return parent;
                    case 3:
                        var teacher = new Teacher
                        {
                            UserId = user.Id,
                            DateofBirth = model.DateofBirth,
                            gender = model.gender,
                            Qualification = model.Qualification,
                            address = model.address
                        };
                        _context.Teachers.Add(teacher);
                        await _context.SaveChangesAsync();
                        return teacher;

                    case 4:
                        var medicalS = new MedicalS
                        {
                            UserId = user.Id,                            
                            gender = model.gender,
                            Qualification = model.Qualification,
                            address = model.address
                        };
                        _context.MedicalS.Add(medicalS);
                        await _context.SaveChangesAsync();
                        return medicalS;
                    case 5:
                        var financeS = new FinanceS
                        {
                            UserId = user.Id,
                            gender = model.gender,
                            Qualification = model.Qualification,
                            address = model.address
                        };
                        _context.FinanceS.Add(financeS);
                        await _context.SaveChangesAsync();
                        return financeS;

                    case 6:
                        var socialS = new SocialS
                        {
                            UserId = user.Id,                            
                            Qualification = model.Qualification,
                            Address = model.address,
                            Gender = model.gender,
                        };
                        _context.SocialS.Add(socialS);
                        await _context.SaveChangesAsync();
                        return socialS;

                    // Handle other account types here...

                    default:
                        return null;
                }
            }

            else
            {
                return null;
            }

        }





        }
}
