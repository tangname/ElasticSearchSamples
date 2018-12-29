using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticeSearch_Service.Dtos
{
    public class PageOut
    {
        public long TotalCount { get; set; }

        public List<IHit<Article>> Documents { get; set; }
    }
}
