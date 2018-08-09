using Dapper;
using DapperFactory;
using Domain;
using IDapperFactory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlMap;
using System;
using System.Linq.Expressions;

namespace TestLambdaClips
{
    [TestClass]
    public class UnitTest1
    {
        //protected IQuerySelect _querySelect;
        //public UnitTest1(IQuerySelect querySelect)
        //{
        //    _querySelect = querySelect;
        //}
        [TestMethod]
        public void TestMethod1()
        {
            IQuerySelect querySelect = new QuerySelect();
            //Tuple<string, DynamicParameters> tuple=OrmBase.LambdaAnalysis<User>(s=>s.Username == "admin" && s.Password == "admin" && s.Sex == Sex.ÄÐ);
            string account = "1";
            string password = "111";
            Sex sex = Sex.ÄÐ;
            User User = querySelect.SelectSingle<User>(s =>s
            .Sex== sex);
        }
    }
}
