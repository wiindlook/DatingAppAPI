using DatingApp.Helper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]//pt a ne folosi de action filter , avem nevoie de action filter pt a lua informatiile doar cand clientul face un request
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController :ControllerBase
    {

    }
}
