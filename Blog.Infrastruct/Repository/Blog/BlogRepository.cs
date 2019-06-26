using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Text;
using Blog.Common;

namespace Blog.Infrastruct
{
    public class BlogRepository : Repository<Domain.Blog, int>, IBlogRepository
    {
        public void InsertWhisper(Domain.Blog blog)
        {
            useTransaction = true;
            CreateConnection(s => { return Insert(s, blog); });
        }
        /// <summary>
        /// 发表微语
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="blog"></param>
        /// <returns></returns>
        private dynamic Insert(IDbConnection dbConnection, Domain.Blog blog)
        {
            Whisper whisper = blog.BlogBase as Whisper;
            Whisper.SetUploadFileGuids(whisper);
            IList<UploadFile> uploadFiles = whisper.UploadFileList;
            string insertFileSql = "INSERT INTO UploadFile(uploadfile_guid,uploadfile_useraccount, uploadfile_savepath, uploadfile_filename, uploadfile_filesize)" +
                " VALUES (@GUID, @Account, @SavePath, @FileName, @FileSize);";
            dbConnection.Execute(insertFileSql, uploadFiles, dbTransaction);
            string insertWhisperSql = "INSERT INTO Whisper(whisper_content, whisper_commentguids, whisper_praisecount, whisper_praiseaccount, whisper_uploadfileguids, whisper_createtime)" +
                " VALUES ( @Content, @Commentguids, @PraiseCount, @PraiseAccount, @UploadfileGuids, NOW());SELECT @@identity";
            int whisperId= dbConnection.ExecuteScalar<int>(insertWhisperSql, whisper, dbTransaction);
            Domain.Blog.SetBlogBaseId(whisperId, blog);
            string insertBlogSql = "INSERT INTO Blog(blog_account, blog_blogbaseid, blog_blogtype, blog_createtime) " +
                "VALUES (@Account, @BlogBaseId, @GetBlogType, NOW());";

            return  dbConnection.Execute(insertBlogSql,blog,dbTransaction);
        }
        /// <summary>
        /// 分页查询微语
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Domain.Blog> SelectByPage()
        {
            string sql = "SELECT * FROM Blog INNER JOIN Whisper ON blog_blogbaseid=whisper_id  WHERE blog_id>0 ORDER BY blog_id desc  LIMIT 10 ";
            IEnumerable<Domain.Blog> d= CreateConnection(s => {
                return Select(s,sql);
            });
            return d;
        }
        private IEnumerable<dynamic> Select(IDbConnection dbConnection,string sql)
        {
            return dbConnection.Query(sql);
        }
    }
}
