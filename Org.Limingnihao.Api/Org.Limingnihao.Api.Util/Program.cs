using Org.Limingnihao.Api.Util.Util;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Org.Limingnihao.Api
{
    public class Program
    {
        static void Main(string[] args)
        {
            //string test = "18456789876";
            Console.WriteLine("------------" + RegexUtil.IsPhone("134567898760"));
            Console.WriteLine("------------" + RegexUtil.IsPhoneMobile("134567898760"));
            Console.WriteLine("------------" + RegexUtil.IsPhoneMobile("13426289258"));

            Console.WriteLine("------------" + RegexUtil.IsPhoneTelecom("134567898760"));
            Console.WriteLine("------------" + RegexUtil.IsPhoneUnicom("134567898760"));

            Console.WriteLine("------------身份证" + RegexUtil.IsIdentityCard("230"));
            Console.WriteLine("------------身份证" + RegexUtil.IsIdentityCard("230123198505282955"));
            Console.WriteLine("------------身份证" + RegexUtil.IsIdentityCard("123456789876543"));
            Console.WriteLine("------------身份证" + RegexUtil.IsIdentityCard("23012319850528295x"));
            Console.WriteLine("------------身份证" + RegexUtil.IsIdentityCard("23012319850528295X"));



            Console.ReadLine();
        }
       
    }
}
