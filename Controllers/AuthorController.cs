using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Personal_EF_API.Interfaces;
using AutoMapper;
using Personal_EF_API.Data.Mappings;
using Personal_EF_API.Data.Mappings.DTOs;
using Personal_EF_API.Data;
using Microsoft.AspNetCore.Authorization;

namespace Personal_EF_API.Controllers
{
    /// <summary>
    /// This is the Endpoint Controller to connect with the Author Table in the database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    //  [ProducesResponseType(StatusCodes.Status200OK)]
    public class AuthorController : ControllerBase
    {

        private readonly IAuthoRepository _authoRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;


        public AuthorController(IAuthoRepository authoRepository, ILoggerService logger, IMapper mapper)
        {
            _authoRepository = authoRepository;
            _logger = logger;
            _mapper = mapper;
        }


        /// <summary>
        /// Get all Writers
        /// </summary>
        /// <returns>List of Authors</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAuthors()
        {
            try
            {
                _logger.LogInfo("Try to Get All Authors");
                var listauthors = await _authoRepository.FindAll();
                var response = _mapper.Map<IList<AuthorDTO>>(listauthors);
                _logger.LogInfo("Sucessfully got All");
                return Ok(response);

            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                return StatusCode(500, "Failed to Get All Authors.Contact Denis");
            }

        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAuthor(int id)
        {
            try
            {
                _logger.LogInfo($"Try to get author with ID: {id} ");
                var author = await _authoRepository.FindById(id);
                if(author == null)
                {
                    _logger.LogWarn($"Id:{id} Author not found");
                    return NotFound();
                }
                var response = _mapper.Map<AuthorDTO>(author);
                _logger.LogInfo($"Sucess! got author with ID : {id}");
                return Ok(response);


            }
            catch(Exception e)
            {
                _logger.LogError($"{e.Message}");
                return StatusCode(500, "Failed to Get All Author by id .Contact Denis");

            }

       
        }

        [HttpPost]
        //[Authorize(Roles ="Admin")]
        public async Task<IActionResult> Create([FromBody] CreatAuthorDTO author)
        {

            try
            {
              
                if(author == null)
                {
                    _logger.LogWarn($"Empty Author object");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var addauthor = _mapper.Map<Author >(author);
                var result = await _authoRepository.Create(addauthor);
                if(result == false)
                {
                    _logger.LogWarn($"Author create failed");

                }
                return Created("Create", new { addauthor });

            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                return StatusCode(500, "Failed to Create Author  .Contact Denis");

            }

        }

        //Update
        /// <summary>
        /// Edit Author
        /// </summary>
        /// <param name="id"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        [Authorize(Roles = "Admin"+"Personal")]
        public async Task<IActionResult> UpdateAuthor( int id , [FromBody] UpdateAuthorDTO author)
        {

            try
            {
                _logger.LogInfo($"Try to Update Author");
                var isExist = await _authoRepository.isExist(id);
                if(isExist == false)
                {
                    return NotFound();
                }

           

                if (id < 1 || author == null || id != author.ID)
                {
                    _logger.LogWarn($"Empty Author object");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var addauthor = _mapper.Map<Author>(author);
                var result = await _authoRepository.Update(addauthor);
                if (result == false)
                {
                    _logger.LogWarn($"Author update failed");

                }
                return NoContent();

            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                return StatusCode(500, "Failed to Update Author  .Contact Denis");

            }

        }

        /// <summary>
        /// Delete author
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]

        public async Task<IActionResult> DeleteAuthor(int id)
        {

            try
            {
                _logger.LogInfo($"Try to Delete Author");

                if (id < 1 )
                {
                    _logger.LogWarn($"Empty Author object");
                    return BadRequest();
                }
                var isExist = await _authoRepository.isExist(id);
                if(isExist == false)
                {
                    _logger.LogWarn($"Empty Author does not exist in DB");
                    return NotFound();
                }
                var author = await _authoRepository.FindById(id);
                var isDeleted = await _authoRepository.Delete(author);
                if(isDeleted == false)
                {
                    return StatusCode(500, "Failed to Delete Author  .Contact Denis");
                }
                return NoContent();

            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                return StatusCode(500, "Failed to Update Author  .Contact Denis");

            }

        }








    }
}
