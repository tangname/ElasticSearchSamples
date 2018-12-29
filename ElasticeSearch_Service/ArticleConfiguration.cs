using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticeSearch_Service
{
    public class ArticleConfiguration
    {
        private static ElasticClient _client;

        /*
         *  链接可以复用，此处使用单例(未严格处理)
         */
        /// <summary>
        /// 创建连接，使用单节点程序池（如果使用集群，需要修改）
        /// </summary>
        /// <returns></returns>
        public static ElasticClient GetClient()
        {
            if (_client == null)
            {
                var uri = new Uri("http://10.1.0.109:9200");
                var pool = new SingleNodeConnectionPool(uri);
                _client = new ElasticClient(new ConnectionSettings(pool));
            }

            return _client;
        }

        /// <summary>
        /// 索引的名称
        /// </summary>
        public const string ArticleIndexName = "article";

    } 
}
