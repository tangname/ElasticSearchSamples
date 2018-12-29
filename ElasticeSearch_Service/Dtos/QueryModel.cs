using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticeSearch_Service.Dtos
{
    /// <summary>
    /// 查询模型，用于页面
    /// </summary>
    public class QueryModel
    {
        public int From
        {
            get
            {
                return (Page - 1) * Size;
            }
        }

        public int Size { get; set; }

        public int Page { get; set; }

        public string Key { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int Sort { get; set; }
    }
}
