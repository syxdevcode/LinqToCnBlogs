using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LinqToCnBlogs
{
    public class CnblogsQueryProvider : QueryProvider
    {
        public override String GetQueryText(Expression expression)
        {
            SearchCriteria criteria;

            // 翻译查询条件
            criteria = new PostExpressionVisitor().ProcessExpression(expression);

            // 生成URL
            String url = PostHelper.BuildUrl(criteria);

            return url;
        }

        public override object Execute(Expression expression)
        {
            String url = GetQueryText(expression);
            IEnumerable<Post> results = PostHelper.PerformWebQuery(url);
            return results;
        }
    }
}
