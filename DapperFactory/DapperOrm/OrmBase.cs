using Dapper;
using DBHelper;
using Domain;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DapperFactory
{
   public abstract class OrmBase
    {
        private static readonly object obj = new object();
        protected static MySqlConnection mySqlConnection = ConnectionProvider.Provider.connection;
        /// <summary>
        /// sql语句
        /// </summary>
        private string Sql;
        /// <summary>
        /// sql条件参数
        /// </summary>
        private DynamicParameters DynamicParameters;
        #region 解析lambda表达式
        /// <summary>
        /// 解析Lambda
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        public void LambdaAnalysis<T>(Expression<Func<T, bool>> expression)
        {
            Tuple<string, DynamicParameters> tuple = null;
            string className = expression.Body.GetType().Name;
            string sql = string.Empty;
            switch (className)
            {
                case "LogicalBinaryExpression":
                case "BinaryExpression":
                    BinaryExpression binaryExpression = expression.Body as BinaryExpression;
                    tuple = Where(binaryExpression.Left, binaryExpression.Right, binaryExpression.NodeType);
                    break;
            }
            Sql = string.Format("{0} where {1} and DeleteTime is null", GetSqlLeft(typeof(T)), tuple.Item1);
            DynamicParameters = tuple.Item2;
        }
        /// <summary>
        /// 递归生成where条件
        /// </summary>
        /// <param name="left">左表达式</param>
        /// <param name="right">又表达式</param>
        /// <param name="expressionType">表达式树节点类型</param>
        /// <param name="className">表达式名字</param>
        /// <param name="dynamicParameters">sql条件参数，此处用来递归添加参数</param>
        /// <param name="paraName">sql参数化</param>
        /// <returns>返回元祖集合，包含where条件和条件参数 </returns>
        public static  Tuple<string, DynamicParameters> Where(Expression left, Expression right, ExpressionType expressionType,DynamicParameters dynamicParameters=null, string paraName = null)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if(dynamicParameters==null)
                dynamicParameters = new DynamicParameters();
            //获取左边条件字段
            string sqlByLeft = SqlFragment(left, dynamicParameters, paraName).Item1;
            //获取条件符号
            string typeCast = ExpressionTypeCast(expressionType);
            //获取右边参数化的条件@para
            string sqlByRight = SqlFragment(right, dynamicParameters, sqlByLeft).Item1;
            // DynamicParameters参数类型
            stringBuilder.Append(sqlByLeft);
            stringBuilder.Append(typeCast);
            stringBuilder.Append(sqlByRight + " ");
            Tuple<string, DynamicParameters> resultMap =
                new Tuple<string, DynamicParameters>(stringBuilder.ToString(), dynamicParameters);
            return resultMap;

        }
        /// <summary>
        /// 递归生成where条件，最先递归left
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <param name="className">表达式名</param>
        /// <param name="paraName">sql参数化名字</param>
        /// <param name="dynamicParameters">sql条件参数，此处用来递归添加参数，防止递归时
        /// 该条件不会被初始化
        /// </param>
        /// <returns></returns>
        public static Tuple<string, DynamicParameters> SqlFragment(Expression expression,DynamicParameters dynamicParameters, string paraName = null)
        {            
            if (dynamicParameters == null)
                dynamicParameters = new DynamicParameters();
            string sqlChip = string.Empty;
            //sql参数元祖
            Tuple<string, DynamicParameters> tupleParameters = null;
            string expName = expression.GetType().Name;
            switch (expName)
            {

                //递归解析表达式的body，直至最小单元
                case "LogicalBinaryExpression":
                case "BinaryExpression":
                case "MethodBinaryExpression":
                    BinaryExpression binaryExpression = expression as BinaryExpression; 
                    tupleParameters=Where(binaryExpression.Left, binaryExpression.Right, binaryExpression.NodeType, dynamicParameters, paraName);
                    break;
                //一组表达式的左节点为PropertyExpression;如：Username=="admin"，通过解析表达式后Username属于PropertyExpression
                case "PropertyExpression":
                    MemberExpression memberExpression = expression as MemberExpression;
                    sqlChip = memberExpression.Member.Name;
                    //返回DynamicParameters到Where方法中，否者会出异常
                    tupleParameters = new Tuple<string, DynamicParameters>(sqlChip, dynamicParameters);
                    break;
                // 一组表达式的左节点为PropertyExpression; 如：Username == "admin"，通过解析表达式后Username属于PropertyExpression
                case "ConstantExpression":
                    ConstantExpression constantExpression = expression as ConstantExpression;
                    if (!string.IsNullOrEmpty(paraName))
                    {
                        sqlChip = $"@{paraName}";
                        object value = constantExpression.Value;
                        //dynamicParameters.AddDynamicParams(new { paraName = value });
                        //AddDynamicParams添加匿名类型，匿名类型无法通过参数命名，
                        //因为最后会变成paraName ="admin"而不是Username="admin";
                        var type = constantExpression.Type.Name;
                        switch(type)
                        {
                            case "String":
                                dynamicParameters.Add(paraName, value, DbType.String);
                                break;
                            case "Int32":
                                dynamicParameters.Add(paraName, value, DbType.Int32);
                                break;
                            default:
                                break;
                        }
                        tupleParameters = new Tuple<string, DynamicParameters>(sqlChip, dynamicParameters);
                    }
                    break;
                //一元运算符，为枚举条件时会执行该处
                //
                case "UnaryExpression":
                    UnaryExpression unaryExpression = expression as UnaryExpression;
                    tupleParameters = SqlFragment(unaryExpression.Operand, dynamicParameters);
                    break;
                case "FieldExpression":
                    MemberExpression mExp = expression as MemberExpression;
                    var member = Expression.Convert(expression, typeof(object));
                    var lambda = Expression.Lambda<Func<object>>(member);
                    var getter = lambda.Compile().Invoke();
                    dynamicParameters.Add(mExp.Member.Name, getter);
                    tupleParameters = new Tuple<string, DynamicParameters>($"@{mExp.Member.Name}", dynamicParameters);
                    break;
            }
            return tupleParameters;
        }
        private static string ExpressionTypeCast(ExpressionType expressionType)
        {
            switch (expressionType)
            {
                case ExpressionType.AndAlso:
                    return "and ";
                case ExpressionType.Equal:
                    return "=";
                default:
                    return "";
            }
        }
        #endregion

        #region 查询
        /// <summary>
        /// 获取查询语句条件左部分
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetSqlLeft(Type type)
        {
            string tableName ="t_"+type.Name.ToLower();
            PropertyInfo[] propertys = type.GetProperties();
            List<string> fieldList = new List<string>();
            foreach (PropertyInfo item in propertys)
                fieldList.Add(item.Name);

            return string.Format("select {0} from {1} ", string.Join(',', fieldList), tableName);
        }

        /// <summary>
        /// 查询单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        public virtual T SelectEntity<T>(Expression<Func<T, bool>> expression)
        {
                LambdaAnalysis(expression);
                T t = default(T);
                t = mySqlConnection.QueryFirstOrDefault<T>(Sql, DynamicParameters);
                return t;
        }

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        public virtual int Count<T>(Expression<Func<T, bool>> expression)
        {
            LambdaAnalysis<T>(expression);
            string tableName = typeof(T).Name;
            string sql = $"select count(*) from {tableName}" + Sql;
            int count=mySqlConnection.Query(sql, DynamicParameters).Count();
            return count;
        }
        #endregion

        #region 新增
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <returns></returns>
        public static string GetInsertSql(Type type)
        {
            string tableName = type.Name;
            PropertyInfo[] propertys = type.GetProperties();
            StringBuilder sbInsert = new StringBuilder();
            sbInsert.Append("insert into t_");
            sbInsert.Append(tableName);
            List<string> fileds = new List<string>();
            List<string> values = new List<string>();
            for (int i = 0; i < propertys.Length; i++)
            {
                PropertyInfo propertyInfo = propertys[i];
                if (propertyInfo.Name == "Id")
                    continue;
                fileds.Add(propertyInfo.Name);
                values.Add("@" + propertyInfo.Name);
            }
            sbInsert.Append("(");
            sbInsert.Append(string.Join(",", fileds));
            sbInsert.Append(") ");
            sbInsert.Append("values (");
            sbInsert.Append(string.Join(",", values));
            sbInsert.Append(")");
            return sbInsert.ToString();
        }
        public virtual async Task<T> Insert<T>(T t)
        {
            string sql = GetInsertSql(typeof(T));
            T entity = await mySqlConnection.ExecuteScalarAsync<T>(sql, t);
            return entity;
        }
        #endregion
    }
}
