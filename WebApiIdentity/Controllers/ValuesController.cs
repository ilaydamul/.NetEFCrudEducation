using Microsoft.AspNetCore.Mvc;
using WebApiIdentity.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


//API'de controller isimleri s takısı ile tanımlanır ve api/ ile endpoint başlaması da bir standarttır.

namespace WebApiIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public IActionResult Get()
        {
            var values = new List<ValueDto>();
            values.Add(new ValueDto { Id = 1, Name = "Name1" });
            values.Add(new ValueDto { Id = 2, Name = "Name2" });

            return Ok(values);// Status Code 300
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")] //Swagger bunsuz çalışmaz.
        public IActionResult Get(int id)
        {
            return Ok(new ValueDto{ Id = id, Name = $"Value{id}" });
        }

        // POST api/<ValuesController>
        [HttpPost]
        public IActionResult Post([FromBody] ValueDto value)
        {
            //bODYDEN GELEN DEĞERLER [From Body] ile yakalarız.
            //Post Created 201 döndürürüz.
            return Created($"api/values/{value.Id}", value);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ValueDto value)
        {
            //Hangi idli kaynak güncellenecek ve güncel değer ne 
            //kaynak güncelleme endpoint,

            return NoContent (); //JSON Güncelleme ve Silme 204 döner.
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, [FromHeader] string lang)
        {
            //İd ile silinecek kaynak bilgisini routedan okuruz.
            return NoContent();//JSON No Content dönüş bir standarttır. Silme işlemlerinde
        }


        [HttpDelete]//Route değeri koymadık queryString oldu
        public IActionResult DeleteByName([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return NotFound();//Kaynak bulunamadı.
            }
            //İd ile silinecek kaynak bilgisini routedan okuruz.
            return NoContent();//JSON No Content dönüş bir standarttır. Silme işlemlerinde
        }
    }
}
