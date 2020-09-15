using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Storage.API.Models;
using Storage.API.Services;

namespace Storage.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class StorageUpdateController : Controller
    {
        private StorageRepository storageRepository;
        private AuthRepository authRepository;

        public StorageUpdateController()
        {
            storageRepository = new StorageRepository();
            authRepository = new AuthRepository();
        }

        [HttpPost]
        public IActionResult Post(int quantity=1, string name="", string location="", string username="", string password="")
        {
            var auth = authRepository.AuthoriseUser(username, password);
            if (!auth.authorised) return BadRequest(auth.errorMessage);
            
			storageRepository.AddItem(quantity, name, location);
            ////if (error) return BadRequest(errorText);
            //else return Ok("Item successfully added");
            return Ok("Item added");
        }

    }
}
