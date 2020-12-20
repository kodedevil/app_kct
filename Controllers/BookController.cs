using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace app.Controllers{
    [ApiController]
    [Route("[controller]")]
    public class BookController: ControllerBase{
        private static readonly List<Book> Books = new List<Book>(){
            new Book(){
                Id = 1,
                Name = "The Innovator's Dilemma",
                Author = "Clayton M",
                PublishedOn = new DateTime(2018,1,1)
            },
            new Book(){
                Id = 2,
                Name = "Monolith to Microservices",
                Author = "Sam Newman",
                PublishedOn = new DateTime(2019,12,1)
            }
        };

        [HttpGet]
        public ActionResult<List<Book>> Get(){
            return Books;
        }

        [HttpGet("{id}")]
        public ActionResult<Book> GetBook(long id)
        {
            Book book = Books.Single(x=>x.Id == id);
            return book;
        }

        [HttpPost]
        public ActionResult<Book> Post(Book book){
            book.Id = Books.Select(x=>x.Id).Max() + 1;
            Books.Add(book);
            return Created(book.Id.ToString(), book);
        }

        [HttpPut] 
        public ActionResult<Book> Put(Book book){
            Book updatedBook = Books.SingleOrDefault(x => x.Id == book.Id);

            if(updatedBook is null){
                return NotFound();
            }

            updatedBook.Name = book.Name;
            updatedBook.Author = book.Author;
            updatedBook.PublishedOn = book.PublishedOn;
            return updatedBook;
        }

        [HttpDelete]
        public ActionResult<long> Delete(long id){
            Book deletedBook = Books.SingleOrDefault(x=>x.Id == id);
            
            if (deletedBook is null)
            {
                return NotFound();
            }

            Books.Remove(deletedBook);
            return id;
        }
    }
}
