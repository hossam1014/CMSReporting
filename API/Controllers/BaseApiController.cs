using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class BaseApiController : ControllerBase
    {
        
    }
}