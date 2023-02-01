using AutoMapper;
using ShoppingList.Business.Repositories.IRepositories;
using ShoppingList.Data;
using ShoppingList.Data.Entities;
using ShoppingList.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShoppingList.Business.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private string secretKey;
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        public AuthRepository(
            ApplicationDbContext db,
            IMapper mapper,
            IConfiguration configuration)
        //UserManager<ApplicationUser> userManager,
        //RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _mapper = mapper;
            _configuration = configuration;
            //_userManager = userManager;
            //_roleManager = roleManager;
            secretKey = _configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.LocalUsers.FirstOrDefault(x => x.UserName == username);

            // return null if user not found
            if (user == null)
                return true;

            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.LocalUsers.SingleOrDefault(x => x.UserName == loginRequestDTO.UserName);
            //var user = _db.ApplicationUsers.SingleOrDefault(x => x.UserName == loginRequestDTO.UserName);
            //bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            //user not found
            if (user == null)
            {
                return null;
            }

            ////user not found
            //if (user == null || isValid == false)
            //{
            //    return null;
            //}


            // user valid ==> generate JWT-Token
            //var roles = await _userManager.GetRolesAsync(user);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.UserName), // add Email etc., what you want
                    new Claim(ClaimTypes.Role,user.Role) // if you have more roles => then add it in a foreach loop
                    // hier nur eine role für einen user
                    //new Claim(ClaimTypes.Role,roles.FirstOrDefault()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                // ..256.. standard zur zeit des videos
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new()
            {
                User = _mapper.Map<UserDTO>(user),
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
            return loginResponseDTO;

        }

        public async Task<UserDTO> Register(RegisterationRequestDTO requestDTO)
        {
            LocalUser userObj = new()
            {
                UserName = requestDTO.UserName,
                Password = requestDTO.Password,
                Name = requestDTO.Name,
                Role = "admin" // hardcoded, sonst automapper
            };

            _db.LocalUsers.Add(userObj);
            _db.SaveChanges();
            userObj.Password = "";
            return _mapper.Map<UserDTO>(userObj);

            //ApplicationUser userObj = new()
            //{
            //    UserName = requestDTO.UserName,
            //    Name = requestDTO.Name,
            //    NormalizedEmail = requestDTO.UserName.ToUpper(),
            //    Email = requestDTO.UserName,
            //};

            //try
            //{
            //    var result = await _userManager.CreateAsync(userObj, requestDTO.Password);
            //    if (result.Succeeded)
            //    {
            //        if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
            //        {
            //            await _roleManager.CreateAsync(new IdentityRole("admin"));
            //            await _roleManager.CreateAsync(new IdentityRole("customer"));
            //        }
            //        await _userManager.AddToRoleAsync(userObj, "admin");

            //        var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == requestDTO.UserName);
            //        return _mapper.Map<UserDTO>(user);
            //    }
            //}
            //catch (Exception e)
            //{

            //}
            //return null;
        }
    }
}
