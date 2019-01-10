using LinqToCnBlogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LinqToCnBlogs
{
    public class PostManager
    {
        private static List<Post> _list;
        private static DateTime _lastModified = DateTime.Now;
        private static volatile object _obj = new object();
        private static readonly string _serviceUrl;

        static PostManager()
        {
            _serviceUrl = "http://wcf.open.cnblogs.com/blog/sitehome/recent/64";

            // 初始加载
            LoadPostFromCnblogs();
        }

        private static void LoadPostFromCnblogs()
        {
            lock (_obj)
            {
                _list = new List<Post>();
                var document = XDocument.Load(_serviceUrl);

                var elements = document.Root.Elements();
                var result = from entry in elements
                             where entry.HasElements == true
                             select new Post
                             {
                                 Id = Convert.ToInt32(entry.Elements()
                                     .SingleOrDefault(x => x.Name.LocalName == "id").Value),

                                 Title = entry.Elements()
                                     .SingleOrDefault(x => x.Name.LocalName == "title").Value,

                                 Published = Convert.ToDateTime(entry.Elements()
                                     .SingleOrDefault(x => x.Name.LocalName == "published").Value),

                                 Diggs = Convert.ToInt32(entry.Elements()
                                     .SingleOrDefault(x => x.Name.LocalName == "diggs").Value),

                                 Views = Convert.ToInt32(entry.Elements()
                                     .SingleOrDefault(x => x.Name.LocalName == "views").Value),

                                 Comments = Convert.ToInt32(entry.Elements()
                                     .SingleOrDefault(x => x.Name.LocalName == "comments").Value),

                                 Summary = entry.Elements()
                                     .SingleOrDefault(x => x.Name.LocalName == "summary").Value,

                                 Href = entry.Elements()
                                     .SingleOrDefault(x => x.Name.LocalName == "link")
                                     .Attribute("href").Value,

                                 Author = entry.Elements()
                                     .SingleOrDefault(x => x.Name.LocalName == "author")
                                     .Elements()
                                     .SingleOrDefault(x => x.Name.LocalName == "name").Value
                             };

                _list.AddRange(result);
                _lastModified = DateTime.Now;
            }
        }

        public static IEnumerable<Post> Posts
        {
            get
            {
                // 一分钟这后再次去博客园取最新的数据
                if (DateTime.Now.AddSeconds(60) > _lastModified)
                {
                    LoadPostFromCnblogs();
                }
                return _list;
            }
        }
    }
}