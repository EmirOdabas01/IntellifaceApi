using AutoMapper;
using Intelliface.BLL.DTOs;
using Intelliface.BLL.Interfaces;
using Intelliface.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace IntellifaceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TrainImageController : ControllerBase
    {
        private readonly ITrainImageService _trainImageService;
        private readonly IMapper _mapper;

        public TrainImageController(ITrainImageService trainImageService, IMapper mapper)
        {
            _trainImageService = trainImageService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMultiple([FromBody] TrainImageCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                foreach (var imageData in dto.ImageData)
                {
                    var trainImage = new EmployeeTrainImage
                    {
                        EmployeeId = dto.EmployeeId,
                        ImageData = imageData
                    };

                    await _trainImageService.AddAsync(trainImage);
                }
                return Ok("Images added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetAllByEmployeeId(int employeeId)
        {
            var images = await _trainImageService.GetAllByEmployeeIdAsync(employeeId);
            var result = images.Select(img => new ReadDto<TrainImageDto>
            {
                Id = img.Id,
                Data = _mapper.Map<TrainImageDto>(img)
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var image = await _trainImageService.GetByIdAsync(id);
            if (image == null) return NotFound("Train image not found.");

            var result = new ReadDto<TrainImageDto>
            {
                Id = image.Id,
                Data = _mapper.Map<TrainImageDto>(image)
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TrainImageDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var entity = _mapper.Map<EmployeeTrainImage>(dto);
                await _trainImageService.AddAsync(entity);
                return Ok("Train image created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _trainImageService.GetByIdAsync(id);
            if (existing == null) return NotFound("Train image not found.");

            await _trainImageService.DeleteAsync(existing);
            return Ok("Train image deleted successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TrainImageDto dto)
        {
            var existing = await _trainImageService.GetByIdAsync(id);
            if (existing == null) return NotFound("Train image not found.");

            try
            {
                _mapper.Map(dto, existing);
                await _trainImageService.UpdateAsync(existing);
                return Ok("Train image updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
