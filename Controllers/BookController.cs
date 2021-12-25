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
using Microsoft.AspNetCore.Hosting;
using System.IO;
namespace Personal_EF_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private readonly IBookRepository _bookRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        //this does not work in webassembly 
        private readonly IWebHostEnvironment _env;

        public BookController(IBookRepository bookRepository, ILoggerService logger, IMapper mapper, IWebHostEnvironment env)
        {
            _bookRepository = bookRepository;
            _logger = logger;
            _mapper = mapper;
            _env = env;
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
                //tova mi trabvq che img ne izlizat an viev page 
                foreach(var data in response)
                {
                    if (string.IsNullOrEmpty(data.Image) == false)
                    {
                        var imgpath = GetImagePath(data.Image);
                        //if it exist in the uloads cuz it gives me error when it does not have fiels in the Uloads folder
                        if (System.IO.File.Exists(imgpath))
                        {
                            byte[] imgbyte = System.IO.File.ReadAllBytes(imgpath);
                            data.Fileimg = Convert.ToBase64String(imgbyte);
                        }

                    }
                }
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
                if(string.IsNullOrEmpty(response.Image) == false)
                {
                  var imgpath =  GetImagePath(book.Image);
                    //if it exist in the uloads cuz it gives me error when it does not have fiels in the Uloads folder
                    if (System.IO.File.Exists(imgpath))
                    {
                        byte[] imgbyte = System.IO.File.ReadAllBytes(imgpath);
                        response.Fileimg = Convert.ToBase64String(imgbyte);
                    }

                }
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
                //here i upload the img in the api folder
                if(string.IsNullOrEmpty(book.Fileimg) == false)
                {

                    //https://docs.microsoft.com/en-us/dotnet/api/system.io.file.writeallbytes?view=net-6.0
                    var imgPath = GetImagePath(book.Image);
                    byte[] imageBytes = Convert.FromBase64String(book.Fileimg);
                    System.IO.File.WriteAllBytes(imgPath, imageBytes);
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
                var oldImage = await _bookRepository.GetImageFileName(id);

                var book = _mapper.Map<Book>(bookupg);
                var result = await _bookRepository.Update(book);
                if (result == false)
                {
                    _logger.LogWarn($"Book update failed");

                }
                //tuka triq stariq img on Uploads
                if (bookupg.Image.Equals(oldImage) == false)
                {
                    if (System.IO.File.Exists(GetImagePath(oldImage)))
                    {
                        System.IO.File.Delete(GetImagePath(oldImage));
                    }
                }
                //tukq pravq new add to Uploads
                if (string.IsNullOrEmpty(bookupg.Fileimg) == false)
                {
                    byte[] imageBytes = Convert.FromBase64String(bookupg.Fileimg);
                    System.IO.File.WriteAllBytes(GetImagePath(bookupg.Image), imageBytes);
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


        private string GetImagePath(string fileName)
        {
            return ($"{_env.ContentRootPath}\\uploads\\{fileName}");
        }
           

    }
}
