using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Storage.API.Models;
using Storage.API.Services;

namespace Storage.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class StorageLookupController : Controller
    {
        private StorageRepository storageRepository;
        private AuthRepository authRepository;

        public StorageLookupController()
        {
            storageRepository = new StorageRepository();
            authRepository = new AuthRepository();
        }

        [HttpGet]
        public IActionResult Get(string name="", string location="", string username="", string password="")
        {
            var auth = authRepository.AuthoriseUser(username, password);
            if (!auth.authorised) return BadRequest(auth.errorMessage);

            var items = storageRepository.GetQueryItems(name, location);
            //var (error, errorText, items) = storageRepository.GetQueryItems(name);
            //if (error) return BadRequest(errorText);
            //else return Ok(items);
            return Ok(items);
        }

    }
}
