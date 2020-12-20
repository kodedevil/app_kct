using System;
using FluentNHibernate.Mapping;

namespace app
{
    public class Book
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime PublishedOn { get; set; }
        public virtual string Author { get; set; }
    }

    public class BookMapping : ClassMap<Book>
    {
        public BookMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Native();
            Map(x => x.Name);
            Map(x => x.PublishedOn);
            Map(x => x.Author);
        }
    }
}