using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LinqToCnBlogs
{
    public class PostSearch : IEnumerable<Post>
    {
        private SearchCriteria _criteria;

        public PostSearch Where(Expression<Func<Post, Boolean>> predicate)
        {
            _criteria = new PostExpressionVisitor().ProcessExpression(predicate);
            return this;
        }

        public PostSearch Select<TResult>(Expression<Func<Post, TResult>> selector)
        {
            return this;
        }

        IEnumerator<Post> IEnumerable<Post>.GetEnumerator()
        {
            return (IEnumerator<Post>)((IEnumerable)this).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            String url = PostHelper.BuildUrl(_criteria);
            IEnumerable<Post> posts = PostHelper.PerformWebQuery(url);

            return posts.GetEnumerator();
        }
    }
}
