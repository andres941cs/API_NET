using API_NET.Context;
using API_NET.Models;
using API_NET.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_NET.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]// PERMITA A CUALQUIE USUARIO ACCEDER
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDb _AppDbContext;
        private readonly Utils _utils;
        public AuthController(AppDb appDbContext, Utils utils)
        {
            _AppDbContext = appDbContext;
            _utils = utils;
        }
        // PARA INDICAR QUE TIPO DE PETICION SE INDICA ENTRE CORCHETES
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> register(User user)
        {
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            //var checkUser = await _AppDbContext.Users.

            var newUser = new User { Name = user.Name, Email = user.Email, Password = user.Password };
            await _AppDbContext.Users.AddAsync(newUser);
            await _AppDbContext.SaveChangesAsync();
            return Ok("Usuario registrado correctamente.");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(User user)
        {
            var checkUser = await _AppDbContext.Users
                .Where(u=>u.Email == user.Email && u.Password == _utils.encryptSHA256(user.Password))
                .FirstOrDefaultAsync();
            if (checkUser != null) return Ok(_utils.generateJWT(checkUser!));
            else return Unauthorized("EMAIL OR PASSWORD INCORRECT");
        }
    }

        //Task<T>: es una promesa de que el método retornará un resultado en el futuro
        //IActionResult: interfaz que representa una respuesta HTTP  -> ok(), NotFound() etc
    }
}
