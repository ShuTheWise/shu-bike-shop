using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ingenico.Direct.Sdk;
using Ingenico.Direct.Sdk.Domain;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace shu_bike_shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhooksController : ControllerBase
    {
        // POST api/<FooController>
        [HttpPost]
        public void Post([FromBody] CreatePaymentResponse createPaymentResponse)
        {
        }
    }
}
