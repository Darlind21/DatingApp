using API.Business_Layer.Infrastructure;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Business_Layer.Services
{
    /*service class to handle JWT tokens 
    -JWT is a small encoded string that proves someone is who they say they are 
        -JWT string is made of 3 parts: 1.Header 2.Payload 3.Signature => They are separated with "."
    1.Header => This part contains info about the type and the algorythm used to sign the token 
    2.Payload => Contains claims (user info => usernames, role, other info) => The payload is encoded but not encrypted meaning anyone can read it
        so its very important not to put passwords/secretes
    3.Signature => This is the part that prevents tampering 
        The Header and Payload are converted to json strings and are combined in a string with "." between them and then using a secret key stored on the server that only the server knwos
        we sign the string using a hashing algorythm

    After a user logs in, the server sends a JWT token using TokenService to the client
    The client now sends this token alongside with every future request for protected endpoints 
    The server parses the JWT, parses the Header and Payload and using the secret key it computes a signature and compares it with the one from the request
        If the signature matches with the one from the token sent access is allowed, otherwise access is not allowed
     */
    public class TokenService(IConfiguration config, UserManager<AppUser> userManager) : ITokenService //Dependency injection from primary constructor
        /*
         * IConfiguration is a built in interface to access config data 
         *An IConfiguration object can read configuration settings from different sources e.g appsettings.json (it builds a tree from the file and we can access even nested data)
         * you access it like a dictionary by providing the key e.g config[tokenKey]
         */
    {
        public async Task <string> CreateToken(AppUser user)
        {
            //we get the TokenKey from appsettings.json
            var tokenKey = config["TokenKey"] ??
                throw new Exception("Cannot access tokenKey from appsettings");

            if (tokenKey.Length < 64)
            {
                throw new Exception("Your tokenKey needs to be longer");
            }

            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            //A SymmetricSecurityKey obj represents a key used for symetric encryption meaning the same key is used to sign and verify the JWT
            //Encoding.UTF8.GetBytes(tokenKey) converts the string into a byte array because cryptographic algorithms do not work with strings
            //This byte array is passed to SymmetricSecurityKey constructor
            //A SymmetricSecurityKey obj simply wraps the byte array and provides methods and metadata for cryptographic operations

            if (user.UserName == null) throw new Exception("No username for user!");

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Name, user.UserName)
            };
            //A claim is a piece of info about the user 
            //Claims become part of the payload of the JWT
            //The Claim class takes 2 parameters ... new Claim (type, value) 
            //  type => what kind of claim it is (NameIdentifier,Email,Role etc.) ...value => the actual data ur assigning to that claim

            var roles = await userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            //this determines how the token is signed

            var tokenDescriptor = new SecurityTokenDescriptor
            //SecurityTokenDescriptor is a helper class used to describe the structure and content of the token we will generate
            //An obj of this type is passes to a JwtSecurityTokenHandler.CreateToken() method which is used to generate the actual jwt
            {
                Subject = new ClaimsIdentity(claims),
                //ClaimsIdentity class is used to hold a collection of claims that describe the user ...Claims => Facts about the user
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds
            };
            //SecurityTokenDescriptor is a helper class used to describe the structure and content of the token we will generate


            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token); //converts the token into a string header.payload.signature
        }
    }
}
