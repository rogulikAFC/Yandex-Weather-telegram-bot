using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
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
            var captchaSecretKey = Environment.GetEnvironmentVariable("RECAPTCHA_SECRET_KEY");

            var captchaUrl = $"https://www.google.com/recaptcha/api/siteverify" +
                $"?secret={captchaSecretKey}" +
                $"&response={user.captchaToken}";

            var httpClient = new HttpClient();

            var captchaResponse = await httpClient.PostAsync(captchaUrl, null);
            captchaResponse.EnsureSuccessStatusCode();
            var captchaResponseContent = await captchaResponse.Content.ReadAsStringAsync();

            var deserializedResponse = JsonConvert
                .DeserializeObject<CaptchaValidationResponseDto>(captchaResponseContent);

            if (deserializedResponse == null)
            {
                throw new Exception("Serialization error");
            }

            var isHuman = deserializedResponse.Success;

            if (!captchaResponse.IsSuccessStatusCode)
            {
                return BadRequest(nameof(user.captchaToken));
            }

            if (isHuman == false)
            {
                return BadRequest(nameof(user.captchaToken));
            }

            var existUser = await _unitOfWork.UserRepository
                .GetUserByTelegramId(user.Id);

            if (existUser != null)
            {
                return BadRequest(nameof(user.Id));
            }

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
