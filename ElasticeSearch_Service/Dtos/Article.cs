using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticeSearch_Service.Dtos
{
    /*
     * 此示例通过特性来生成文档索引结构
     * 
     */

    /// <summary>
    /// 文章实体
    /// </summary>
    [ElasticsearchType(IdProperty = "Id", Name = ArticleConfiguration.ArticleIndexName)]
    public class Article
    {
        /// <summary>
        /// 主键Id，标记为不索引
        /// </summary>
        [Keyword(Index = false)]
        public Guid Id { get; set; }
        /// <summary>
        /// 标题，标记为全文索引
        /// </summary>
        [Text]
        public string Title { get; set; }
        /// <summary>
        /// 作者，不指定，默认为全文索引
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 类型，标记为全文索引
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 下载次数
        /// </summary>
        public int Downloads { get; set; }
        /// <summary>
        /// 浏览次数
        /// </summary>
        public int Browses { get; set; }
        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool IsRecommend { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creater { get; set; }

        /// <summary>
        /// 评分，临时使用，不存储到文档
        /// </summary>
        [Text(Ignore = true)]
        public float Score { get; set; }
    }
}
