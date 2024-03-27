using Microsoft.AspNetCore.Mvc;
using RGB.Back.Models;
using RGB.Back.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RGB.Back.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MemberTagController : ControllerBase
	{
		private readonly RizzContext _context;
		private readonly MemberTagService _service;

		public MemberTagController(RizzContext context)
		{
			_context = context;
			_service = new MemberTagService(context);
		}

		// GET: api/<MemberTagController>
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/<MemberTagController>/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return "value";
		}

		// POST api/<MemberTagController>
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/<MemberTagController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<MemberTagController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
