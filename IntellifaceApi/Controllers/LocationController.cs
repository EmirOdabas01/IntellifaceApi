using AutoMapper;
using Intelliface.BLL.Interfaces;
using Intelliface.DTOs.Location;
using Intelliface.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntellifaceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly IMapper _mapper;

        public LocationController(ILocationService locationService, IMapper mapper)
        {
            _locationService = locationService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var locations = await _locationService.GetAllLocationsAsync();
            var locationDtos = _mapper.Map<List<LocationReadDto>>(locations);
            return Ok(locationDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var location = await _locationService.GetLocationByIdAsync(id);
            if (location == null)
                return NotFound("not found");

            var locationDto = _mapper.Map<LocationReadDto>(location);
            return Ok(locationDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LocationCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var location = _mapper.Map<Location>(dto);
            await _locationService.AddLocationAsync(location);
            return Ok("succesfull.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LocationUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id does not match.");

            var existing = await _locationService.GetLocationByIdAsync(id);
            if (existing == null)
                return NotFound("not found.");

            _mapper.Map(dto, existing);
            await _locationService.UpdateLocationAsync(existing);
            return Ok("updatedd");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _locationService.GetLocationByIdAsync(id);
            if (existing == null)
                return NotFound("not foundd.");

            await _locationService.DeleteLocationAsync(id);
            return Ok("deleted.");
        }
    }
}
