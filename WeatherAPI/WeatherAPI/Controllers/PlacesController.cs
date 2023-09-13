using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WeatherAPI.DAL;
using WeatherAPI.Entities;
using WeatherAPI.Models;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlacesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));

            _unitOfWork = unitOfWork
                ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        [HttpGet("by_user/{userId}")]
        public async Task<ActionResult<IEnumerable<PlaceDto>>> GetPlaces(long userId)
        {
            var places = await _unitOfWork.PlaceRepository
                .GetPlacesOfUserAsync(userId);

            if (places == null)
            {
                return NotFound(nameof(userId));
            }

            var placeDtos = new List<PlaceDto>();

            foreach (var place in places)
            {
                var placeDto = _mapper.Map<PlaceDto>(place);

                placeDtos.Add(placeDto);
            }

            return placeDtos;
        }

        [HttpGet("{placeId}", Name = "GetPlaceById")]
        public async Task<ActionResult<PlaceDto>> GetPlaceById(Guid placeId)
        {
            var place = await _unitOfWork.PlaceRepository
                .GetByIdAsync(placeId);

            if (place == null)
            {
                return NotFound(nameof(placeId));
            }

            var placeDto = _mapper.Map<PlaceDto>(place);

            return placeDto;
        }

        [HttpPost]
        public async Task<ActionResult<PlaceDto>> CreatePlace(
            PlaceForCreateDto place)
        {
            var user = await _unitOfWork.UserRepository
                .GetUserByTelegramId(place.UserId);

            if (user == null)
            {
                return NotFound(nameof(place.UserId));
            }

            var placeEntity = _mapper.Map<Place>(place);

            Console.WriteLine(placeEntity.Name);

            _unitOfWork.PlaceRepository.Add(placeEntity);
            await _unitOfWork.SaveChangesAsync();

            var placeDto = _mapper.Map<PlaceDto>(placeEntity);

            return CreatedAtAction("GetPlaceById", new
            {
                PlaceId = placeEntity.Id,
            },
            placeDto);
        }

        [HttpPut("{placeId}")]
        public async Task<ActionResult> PutPlaceById(
            Guid placeId, PlaceForUpdateDto place)
        {
            var placeEntity = await _unitOfWork.PlaceRepository
                .GetByIdAsync(placeId);

            if (placeEntity == null)
            {
                return NotFound(nameof(placeId));
            }

            _mapper.Map(place, placeEntity);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{placeId}")]
        public async Task<ActionResult> DeletePlace(Guid placeId)
        {
            var place = await _unitOfWork.PlaceRepository
                .GetByIdAsync(placeId);

            if (place == null)
            {
                return NotFound(nameof(placeId));
            }

            _unitOfWork.PlaceRepository
                .DeleteAsync(place);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    } 
}
