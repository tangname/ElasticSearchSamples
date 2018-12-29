using ElasticeSearch_Service.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticeSearch_Service
{
    /// <summary>
    /// 从文件中获取数据
    /// </summary>
    public class Dumps
    {
        /// <summary>
        /// 从文件获取数据，真实项目请从数据库分页获取
        /// </summary>
        /// <returns></returns>
        public List<Article> LoadAll()
        {
            var articles = new List<Article>();

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Datas\\Document.csv");

            var code = Encoding.GetEncoding("gb2312");
            using (var reader = new StreamReader(filePath, code))
            {
                var index = 0;
                while (!reader.EndOfStream)
                {
                    index++;
                    var lineData = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(lineData)) continue;

                    var datas = lineData.Split(',');

                    var article = new Article()
                    {
                        Id = Guid.Parse(datas[0]),
                        Title = datas[1],
                        Author = datas[2],
                        Description = datas[3],
                        TypeName = datas[4],
                        Downloads = int.Parse(datas[5]),
                        Browses = int.Parse(datas[6]),
                        IsRecommend = datas[7] == "1",
                        CreateTime = DateTime.Parse(datas[8]),
                        Creater = datas[9]
                    };

                    articles.Add(article);
                }
            }

            return articles;
        }
    }
}
