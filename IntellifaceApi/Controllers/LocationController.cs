using AutoMapper;
using Intelliface.BLL.DTOs;
using Intelliface.BLL.Interfaces;
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
            var result = locations.Select(a => new ReadDto<LocationDto>
            {
                Id = a.Id,
                Data = _mapper.Map<LocationDto>(a)
            }).ToList();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var location = await _locationService.GetLocationByIdAsync(id);
            if (location == null) return NotFound("Location not found.");

            var locationDto = _mapper.Map<LocationDto>(location);
            return Ok(locationDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LocationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var location = _mapper.Map<Location>(dto);
            await _locationService.AddLocationAsync(location);
            return Ok("Location created successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LocationDto dto)
        {
            var existing = await _locationService.GetLocationByIdAsync(id);
            if (existing == null) return NotFound("Location not found.");

            _mapper.Map(dto, existing);
            await _locationService.UpdateLocationAsync(existing);
            return Ok("Location updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _locationService.GetLocationByIdAsync(id);
            if (existing == null)
                return NotFound("Location not found.");

            await _locationService.DeleteLocationAsync(id);
            return Ok("Location deleted successfully.");
        }
    }
}
