using ElasticeSearch_Service.Dtos;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticeSearch_Service
{
    public class ESProvider
    {
        /// <summary>
        /// 初始化索引库
        /// </summary>
        public void InitIndex()
        {
            var client = ArticleConfiguration.GetClient();
            var indexName = ArticleConfiguration.ArticleIndexName;

            //判断索引是否存在
            var indexExsistResponse = client.IndexExists(indexName);
            if (!indexExsistResponse.IsValid)
            {
                throw indexExsistResponse.OriginalException;
            }

            //判断索引是否存在，存在，清空索引库
            if (indexExsistResponse.Exists)
            {
                client.DeleteIndex(indexName);
            }

            //重新创建索引库
            var descriptor = new CreateIndexDescriptor(indexName);
            //通过实体创建自动隐射
            descriptor.Mappings(p => p.Map<Article>(m => m.AutoMap()));
            client.CreateIndex(descriptor);
        }

        /// <summary>
        /// 索引数据
        /// </summary>
        public void IndexDumps()
        {
            var client = ArticleConfiguration.GetClient();
            var indexName = ArticleConfiguration.ArticleIndexName;

            var articles = new Dumps().LoadAll();

            var bulk = new BulkRequest(indexName);
            articles.ForEach(article =>
            {
                if (bulk.Operations == null)
                {
                    bulk.Operations = new List<IBulkOperation>();
                }

                var op = new BulkIndexOperation<Article>(article);
                bulk.Operations.Add(op);

                //两千条记录批量提交
                if (bulk.Operations.Count % 2000 == 0)
                {
                    client.Bulk(bulk);

                    bulk = new BulkRequest(indexName);
                }
            });

            if (bulk.Operations != null && bulk.Operations.Count > 0)
            {
                client.Bulk(bulk);
            }

            Console.WriteLine($"初始化完成，数据量：{articles.Count}");
        }

        /// <summary>
        /// 搜索数据
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public PageOut Search(QueryModel input)
        {
            var client = ArticleConfiguration.GetClient();

            var multiMatch = new MultiMatchQueryDescriptor<Article>();
            multiMatch.Fields(f => f
                        .Field(p => p.Title, 1.2) //设置标题的评分高于描述
                        .Field(p => p.Description, 0.8)
                        .Field(p => p.Author)
                       ) //多字段搜索
                      .Operator(Operator.Or) //多个字段使用或
                      .Query(input.Key); //查询关键字

            var filter = new QueryContainerDescriptor<Article>();
            if (input.BeginDate.HasValue)
            {
                filter.DateRange(r => r.Field(p => p.CreateTime).GreaterThanOrEquals(input.BeginDate.Value));
            }

            if (input.EndDate.HasValue)
            {
                filter.DateRange(r => r.Field(p => p.CreateTime).LessThan(input.EndDate.Value));
            }

            var result = client.Search<Article>(s => s
                .Index(ArticleConfiguration.ArticleIndexName)//设置索引
                .Size(input.Size)
                .From(input.From)
                .Query(q => q
                    .MultiMatch(m => multiMatch)
                    &&
                    q.Bool(b => b.Filter(f => filter))
                )
            //.Sort(sort =>
            //{
            //    switch (input.Sort)
            //    {
            //        case 0: return sort.Descending(p => p.CreateTime);
            //        case 1: return sort.Descending(p => p.Downloads);
            //        case 2: return sort.Descending(p => p.Browses);
            //    }

            //    return sort.Descending(p => p.CreateTime);
            //}) //不使用排序，否则搜索的结果会不准确
            );

            //返回结果文档
            return new PageOut()
            {
                Documents = result.Hits.ToList(),
                TotalCount = result.Total,
            };
        }

        /// <summary>
        /// 通过ID获取文档
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Article GetArticle(Guid id)
        {
            var client = ArticleConfiguration.GetClient();

            var path = new DocumentPath<Article>(id);
            path.Index(ArticleConfiguration.ArticleIndexName);

            var result = client.Get(path);

            return result.Source;
        }
    }
}
