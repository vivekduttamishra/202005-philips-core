using BooksWebCore.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BooksWebCore.Controllers
{
    public class AccountController : Controller
    {
        IConfiguration _configuration;
        public AccountController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        //A Web Api Authentication usign a Token

        [HttpPost]
        public async Task<IActionResult> Token( [FromBody] LoginViewModel loginInfo)        
        {
            

            if (ModelState.IsValid)
            {
                var role = loginInfo.Password.Contains("root") ? "Admin" : "Member";
                //var user = await GetUser(loginInfo.Email, loginInfo.Password);
                if(loginInfo.UserName.Length==loginInfo.Password.Length && loginInfo.UserName!=loginInfo.Password)
                {
                    //create claims details based on the user information
                    //Realworld. This comes from a data base
                    //Here we add dummies
                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", loginInfo.UserName),
                    new Claim("Email", $"{loginInfo.UserName.ToLower()}@booksweb.org"),
                    new Claim("role", role) //Important for authorization
                   };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest("Invalid or Missing Details");
            }
        }


        //A Web Application Authentication 
        //Using a login Form
        
        public IActionResult Login(String returnUrl)
        {

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(String returnUrl, LoginViewModel loginUser)
        {
            //real logic story
            if (ModelState.IsValid)
            {
                int index = loginUser.Email.IndexOf("@");

                string userName = loginUser.Email.Substring(0,index); 
                if (userName.Length == loginUser.Password.Length && loginUser.Email != loginUser.Password)
                {
                    var principal = BuildPrincipal(userName, loginUser.Password);
                    await HttpContext.SignInAsync(principal);
                    return Redirect("/");
                }
                else
                    ModelState.AddModelError("*", "Invalid Credentials");
            }

            Response.StatusCode = 400;
            ViewBag.ReturnUrl = returnUrl;
            return View(loginUser);
        }

        public ClaimsPrincipal BuildPrincipal(String name,String password)
        {
            //Step 0 
            //validate user using user name and password!

            String role = password.Contains("root") ? "Admin" : "Member";

            //Step1 - Every user can have multiple claims
            Claim[] personalClaims = {
                new Claim("Name",name),
                new Claim("Address","Bangalore"), //you should pull from database
                new Claim("Age","100"),
                new Claim("role",role)   //IMPORTANT key "role". It defines Authorization roles

            };

            //Step2 - Claims must be verified by some ClaimIdentity
            ClaimsIdentity booksWebAuthority = new ClaimsIdentity(personalClaims, "BooksWebIdentity");
            
            //There can be multiple claims verified by different dientity
            Claim[] panClaims = {
                new Claim("Name",name),
                new Claim("Email",$"{name.ToLower().Replace(' ','.')}@booksweb.org"),
                new Claim("PanCard","ABCDE1234X"),
                new Claim("BirthDate","1900/01/01")
            };

            var incomeTaxAuthority = new ClaimsIdentity(panClaims, "IncomeTaxDepartment");


            //Step 3 - Based On the Claims, Define the Principal --> The person
            // The user may get claims veriifed by different Identity Provider
            ClaimsPrincipal user = new ClaimsPrincipal(new[] { booksWebAuthority, incomeTaxAuthority });

            return user;
        }



        // A Dummy Free Pass!

        public async Task<IActionResult> FreePass(String returnUrl)
        {
            //Step 0 
            //validate user using user name and password!

            //Step1 - Every user can have multiple claims
            Claim[] personalClaims = {
                new Claim("Name","Vivek Dutta Mishra"),
                new Claim("Address","Bangalore"),
                new Claim("Age","100")
            };

            //Step2 - Claims must be verified by some ClaimIdentity
            ClaimsIdentity booksWebAuthority = new ClaimsIdentity(personalClaims,"BooksWebIdentity");

            //There can be multiple claims verified by different dientity
            Claim[] panClaims = {
                new Claim("Name","Vivek Dutta Mishra"),
                new Claim("PanCard","ABCDE1234X"),
                new Claim("BirthDate","1900/01/01")
            };

            var incomeTaxAuthority = new ClaimsIdentity(panClaims, "IncomeTaxDepartment");


            //Step 3 - Based On the Claims, Define the Principal --> The person
            // The user may get claims veriifed by different Identity Provider
            ClaimsPrincipal user = new ClaimsPrincipal(new[] { booksWebAuthority, incomeTaxAuthority });

            //Step 4 - Setup a Cookie with all this details encoded in it
            await HttpContext.SignInAsync(user); //Ok So now user is signed in

            return Redirect("/account/success");

        }

        [Authorize]
        public IActionResult Success()
        {
            var claims = HttpContext.User.Claims;

            return Ok(claims);
        }

        public async Task<IActionResult> Exit()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }


    }
}
