using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Storage.API.Models;
using Storage.API.Services;

namespace Storage.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class StorageResetController : ControllerBase
    {
        private StorageRepository storageRepository;
        private AuthRepository authRepository;

        public StorageResetController()
        {
            storageRepository = new StorageRepository();
            authRepository = new AuthRepository();
        }

        [HttpDelete]
        public IActionResult Delete(string username="", string password="")
        {
            var auth = authRepository.AuthoriseUser(username, password);
            if (!auth.authorised) return BadRequest(auth.errorMessage);

            storageRepository.Reset();
            return Ok("Database reset");
        }
    }
}
