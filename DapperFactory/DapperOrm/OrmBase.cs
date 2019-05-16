using CommonHelper;
using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DapperFactory
{
    public abstract class OrmBase
    {
        protected static MySqlConnection mySqlConnection = ConnectionProvider.connection;
        /// <summary>
        /// sql语句
        /// </summary>
        private string sql;
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
            Tuple<string, DynamicParameters> tuple =null;
            ExpressionType expressionType = expression.Body.NodeType;
            string sql = string.Empty;
            BinaryExpression binaryExpression = expression.Body as BinaryExpression;
            tuple = Where(binaryExpression.Left, binaryExpression.Right, binaryExpression.NodeType);
            sql = string.Format("{0} where {1} ", CreateSelectSql(typeof(T)), tuple.Item1);
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
            //通过表达式左边条件字段
            string sqlField = CreateCondition(left, dynamicParameters, paraName).Item1;
            //获取条件符号
            string typeCast = ExpressionTypeCast(expressionType);
            //获取右边参数化的条件@para
            string sqlValue = CreateCondition(right, dynamicParameters, sqlField).Item1;
            // DynamicParameters参数类型
            stringBuilder.Append(sqlField);
            stringBuilder.Append(typeCast);
            stringBuilder.Append(sqlValue + " ");
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
        public static Tuple<string, DynamicParameters> CreateCondition(Expression expression,DynamicParameters dynamicParameters, string paraName = null)
        {            
            if (dynamicParameters == null)
                dynamicParameters = new DynamicParameters();
            string sqlChip = string.Empty;
            //sql参数元祖
            Tuple<string, DynamicParameters> tupleParameters = null;
            ExpressionType expressionType = expression.NodeType;
            switch (expressionType)
            {
                case ExpressionType.MemberAccess:              
                    MemberExpression memberExpression = expression as MemberExpression;//表达式类型，如s=>s.Account==user.Account的user.Account的Expression
                    PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
                    MemberExpression subExpression = memberExpression.Expression as MemberExpression;
                    sqlChip = memberExpression.Member.Name;
                    tupleParameters = new Tuple<string, DynamicParameters>(sqlChip, dynamicParameters);
                    if (subExpression == null)//不为null代表为表达式右部分，
                        return tupleParameters;
                    var member = Expression.Convert(expression, typeof(object));
                    var lambda = Expression.Lambda<Func<object>>(member);
                    var getter = lambda.Compile().Invoke();
                    dynamicParameters.Add(memberExpression.Member.Name, getter);
                    tupleParameters = new Tuple<string, DynamicParameters>($"@{memberExpression.Member.Name}", dynamicParameters);
                    return tupleParameters;
                case ExpressionType.Equal:
                    BinaryExpression binaryExpression = expression as BinaryExpression;
                    tupleParameters=Where(binaryExpression.Left, binaryExpression.Right, binaryExpression.NodeType, dynamicParameters, paraName);
                    return tupleParameters;
                case ExpressionType.Convert://表达式包含枚举或者其它的一元操作符操作，如s.Sex==Sex.男，此处解析s.Sex表达式
                    UnaryExpression unaryExpression = expression as UnaryExpression;
                    tupleParameters = CreateCondition(unaryExpression.Operand, dynamicParameters);
                    return tupleParameters;
                case ExpressionType.Constant://枚举值或者其它值的一元操作符操作，Sex.男
                    ConstantExpression constantExpression1 = expression as ConstantExpression;
                    if (!string.IsNullOrEmpty(paraName))
                    {
                        sqlChip = $"@{paraName}";
                        object value1 = constantExpression1.Value;
                        var type = constantExpression1.Type.Name;
                        switch (type)
                        {
                            case "String":
                                dynamicParameters.Add(sqlChip, value1, DbType.String);
                                break;
                            case "Int32":
                                dynamicParameters.Add(sqlChip, value1, DbType.Int32);
                                break;
                            default:
                                break;
                        }
                        tupleParameters = new Tuple<string, DynamicParameters>(sqlChip, dynamicParameters);
                    }
                    return tupleParameters;
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
        /// 获取查询语句,不带查询条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string CreateSelectSql(Type type)
        {
            string tableName ="t_"+type.Name.ToLower();
            PropertyInfo[] propertys = type.GetProperties();
            List<string> fieldList = new List<string>();
            foreach (PropertyInfo item in propertys)
                fieldList.Add(item.Name);

            return string.Format("select {0} from {1} ", string.Join(',', fieldList), tableName);
        }
        /// <summary>
        /// 返回类型名字和属性数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static Tuple<string, PropertyInfo[]> GetPropertyInfos<T>()
        {
            var type = typeof(T);
            string tableName = type.Name;
            PropertyInfo[] propertys = type.GetProperties();
            Tuple<string, PropertyInfo[]> tuple = new Tuple<string, PropertyInfo[]>(tableName, propertys);
            return tuple;
        }
        /// <summary>
        /// 返回insert语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="fieldNameBegin">字段名前缀</param>
        /// <returns></returns>
        private static string InsertSqlExpress<T>(string fieldNameBegin)
        {
            Tuple<string, PropertyInfo[]> tuple = GetPropertyInfos<T>();
            StringBuilder sb = new StringBuilder();
            sb.Append("insert into ");
            sb.Append(typeof(T).GetTableName());
            sb.Append(tuple.Item1);
            PropertyInfo propertyInfo;
            List<string> tableNames = new List<string>();
            List<string> values = new List<string>();
            for (int i = 0; i < tuple.Item2.Length; i++)
            {
                propertyInfo = tuple.Item2[i];
                if (propertyInfo.Name == "Id")
                    continue;
                tableNames.Add(fieldNameBegin + "_" + propertyInfo.Name);
            }
            sb.Append("(");
            sb.Append(string.Join(",", tableNames));
            sb.Append(") ");
            sb.Append("values (");
            sb.Append(string.Join(",", values));
            sb.Append(")");
            string sql = sb.ToString();
            return sql;
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
                t = mySqlConnection.QueryFirstOrDefault<T>(sql, DynamicParameters);
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
            string selectSql = $"select count(*) from {tableName}" + sql;
            int count=mySqlConnection.Query(selectSql, DynamicParameters).Count();
            return count;
        }
        #endregion

        #region 新增
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <returns></returns>
        public static string CreateInsertSql(Type type)
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
            string sql = CreateInsertSql(typeof(T));
            T entity = await mySqlConnection.ExecuteScalarAsync<T>(sql, t);
            return entity;
        }
        #endregion
    }
}
