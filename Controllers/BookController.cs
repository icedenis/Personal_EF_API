using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Personal_EF_API.Interfaces;
using AutoMapper;
using Personal_EF_API.Data.Mappings.DTOs;
using Personal_EF_API.Data;

namespace Personal_EF_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private readonly IBookRepository _bookRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;


        public BookController(IBookRepository bookRepository, ILoggerService logger, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all Books
        /// </summary>
        /// <returns>List of Authors</returns>
        [HttpGet]
        public async Task<IActionResult> GetBookList()
        {
            try
            {
                _logger.LogInfo("Try to Get All Authors");
                var listbooks = await _bookRepository.FindAll();
                var response = _mapper.Map<IList<BookDTO>>(listbooks);
                _logger.LogInfo("Sucessfully got All");
                return Ok(response);

            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                return StatusCode(500, "Failed to Get All Books.Contact Denis");
            }

        }

        /// <summary>
        /// Get book by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            try
            {
                _logger.LogInfo($"Try to get Book with ID: {id} ");
                var book = await _bookRepository.FindById(id);
                if (book == null)
                {
                    _logger.LogWarn($"Id:{id} Boook not found");
                    return NotFound();
                }
                var response = _mapper.Map<BookDTO>(book);
                _logger.LogInfo($"Sucess! got Book with ID : {id}");
                return Ok(response);


            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                return StatusCode(500, "Failed to Get All book by id .Contact Denis");

            }


        }
        //Create Book
        /// <summary>
        /// Create Book
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookDTO book)
        {

            try
            {

                if (book == null)
                {
                    _logger.LogWarn($"Empty Book object");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"Validation incomplate");
                    return BadRequest(ModelState);
                }
                var addbook = _mapper.Map<Book>(book);
                var result = await _bookRepository.Create(addbook);
                if (result == false)
                {
                    _logger.LogWarn($"Book create failed");
                    throw new Exception();

                }
                return Created("Create", new { addbook });

            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                return StatusCode(500, "Failed to Create Book  .Contact Denis");

            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookDTO bookupg)
        {

            try
            {
                _logger.LogInfo($"Try to Update Book");
                var isExist = await _bookRepository.isExist(id);
                if (isExist == false)
                {
                    return NotFound();
                }



                if (id < 1 || bookupg == null || id != bookupg.Id)
                {
                    _logger.LogWarn($"Empty Book object");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var book = _mapper.Map<Book>(bookupg);
                var result = await _bookRepository.Update(book);
                if (result == false)
                {
                    _logger.LogWarn($"Book update failed");

                }
                return NoContent();

            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                return StatusCode(500, "Failed to Update Book  .Contact Denis");

            }

        }

        /// <summary>
        /// Delete Book
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteBook(int id)
        {

            try
            {
                _logger.LogInfo($"Try to Delete Book");

                if (id < 1)
                {
                    _logger.LogWarn($"Empty Book object");
                    return BadRequest();
                }
                var isExist = await _bookRepository.isExist(id);
                if (isExist == false)
                {
                    _logger.LogWarn($"Empty Book does not exist in DB");
                    return NotFound();
                }
                var book = await _bookRepository.FindById(id);
                var isDeleted = await _bookRepository.Delete(book);
                if (isDeleted == false)
                {
                    return StatusCode(500, "Failed to Delete Book  .Contact Denis");
                }
            
                return NoContent();

            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                return StatusCode(500, "Failed to Update Book  .Contact Denis");

            }

        }




    }
}
