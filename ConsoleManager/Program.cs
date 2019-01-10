using System;
using System.Linq;
using LinqToCnBlogs;

namespace ConsoleManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var provider = new CnblogsQueryProvider();
            var queryable = new Query<Post>(provider);

            var query =
                from p in queryable
                where p.Diggs >= 10 &&
                      p.Comments > 10 &&
                      p.Views > 10 &&
                      p.Comments < 20
                select p;

            var list = query.ToList();


            Console.ReadLine();
        }
    }
}
