using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace app.Controllers
{
    public class Helper
    {
        private static ISessionFactory _sessionFactory;
        private static string _connectionString;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = Fluently.Configure()
                        .Database(MsSqlConfiguration.MsSql2012.ConnectionString(_connectionString))
                        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Book>())
                        .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
                        .BuildSessionFactory();
                }

                return _sessionFactory;
            }
        }

        public static ISession OpenSession(string connectionString)
        {
            _connectionString = connectionString;
            return SessionFactory.OpenSession();
        }
    }

    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private ISession Session;

        public BookController()
        {
            Session = Helper.OpenSession("Server=192.168.1.50;User Id=AURORA;Password=MachDatum.1;Database=kct;Connection Timeout=30;");
        }

        [HttpGet]
        public ActionResult<List<Book>> Get()
        {
            List<Book> books = Session.Query<Book>().ToList();
            return books;
        }

        [HttpGet("{id}")]
        public ActionResult<Book> GetBook(long id)
        {
            Book book = Session.Get<Book>(id);
            return book;
        }

        [HttpPost]
        public ActionResult<Book> Post(Book book)
        {
            Session.Save(book);
            Session.Flush();
            return Created(book.Id.ToString(), book);
        }

        [HttpPut]
        public ActionResult<Book> Put(Book book)
        {
            Book updatedBook = Session.Get<Book>(book.Id);

            if (updatedBook is null)
            {
                return NotFound();
            }

            updatedBook.Name = book.Name;
            updatedBook.Author = book.Author;
            updatedBook.PublishedOn = book.PublishedOn;

            Session.Update(updatedBook);
            Session.Flush();
            return updatedBook;
        }

        [HttpDelete]
        public ActionResult<long> Delete(long id)
        {
            Book deletedBook = Session.Get<Book>(id);

            if (deletedBook is null)
            {
                return NotFound();
            }

            Session.Delete(deletedBook);
            Session.Flush();
            return id;
        }
    }
}
