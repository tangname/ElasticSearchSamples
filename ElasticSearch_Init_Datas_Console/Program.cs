using ElasticeSearch_Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch_Init_Datas_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Begining init index.");
            Console.WriteLine("Press enter continue...");
            Console.ReadLine();

            //重建索引
           // new IndexProvider().Init();

            //初始化数据
            new ESProvider().IndexDumps();

            Console.WriteLine("init completed!");
            Console.ReadLine();
        }
    }
}
