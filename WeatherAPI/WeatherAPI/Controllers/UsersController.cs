using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherAPI.DAL;
using WeatherAPI.Entities;
using WeatherAPI.Models;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{pageNum}/{pageSize}")]
        public async Task<ActionResult<IEnumerable<UserWithoutPlacesDto>>> GetUsers(
            int pageNum, int pageSize)
        {
            var users = await _unitOfWork.UserRepository
                .GetUsersAsync(pageNum, pageSize);

            var userDtos = new List<UserWithoutPlacesDto>();

            foreach (var user in users)
            {
                var userDto = _mapper.Map<UserWithoutPlacesDto>(user);

                userDtos.Add(userDto);
            }

            return userDtos;
        }

        [HttpGet("{userId}", Name = "GetUserById")]
        public async Task<ActionResult<UserDto>> GetUserById(long userId)
        {
            var user = await _unitOfWork.UserRepository
                .GetUserByTelegramId(userId);

            if (user == null)
            {
                return NotFound(nameof(userId));
            }

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> RegistrateUser(
            UserForRegistrationDto user)
        {
            var userEntity = _mapper.Map<User>(user);

            _unitOfWork.UserRepository.Add(userEntity);
            await _unitOfWork.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(userEntity);

            return CreatedAtAction("GetUserById", new
            {
                UserId = userEntity.Id
            },
            userDto);
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(
            long userId)
        {
            var user = await _unitOfWork.UserRepository
                .GetUserByTelegramId(userId);

            if (user == null)
            {
                return NotFound(nameof(userId));
            }

            _unitOfWork.UserRepository
                .DeleteAsync(user);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
