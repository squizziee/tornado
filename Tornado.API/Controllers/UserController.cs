using Microsoft.AspNetCore.Mvc;
using Tornado.Application.UseCases.Interfaces;
using Tornado.Contracts.Requests;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tornado.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IRegisterUserUseCase _registerUserUseCase;
        public UserController(IRegisterUserUseCase registerUserUseCase) { 
            _registerUserUseCase = registerUserUseCase;
        }
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost]
        public async void Post([FromForm] RegisterUserRequest request, CancellationToken cancellationToken)
        {
            await _registerUserUseCase.ExecuteAsync(request, cancellationToken);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
