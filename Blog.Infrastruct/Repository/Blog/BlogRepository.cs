using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Text;
using Blog.Common;
using System.Linq;
using Blog.Domain.Core;

namespace Blog.Infrastruct
{
    public class BlogRepository : Repository<Domain.Blog, int>, IBlogRepository
    {
        private readonly IUploadFileRepository _uploadFileRepository;
        private readonly ICommentRepository _commentRepository;
        public BlogRepository(IUploadFileRepository uploadFileRepository, ICommentRepository commentRepository)
        {
            _uploadFileRepository = uploadFileRepository;
            _commentRepository = commentRepository;
        }
        #region 结果转换
        /// <summary>
        /// 将查询结果转为实体
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private Domain.Blog Map(dynamic d, IList<UploadFile> uploadFileList, IList<Comment> commentList)
        {
            string uploadFileGuids = d.whisper_uploadfileguids;
            string commentGuids = d.whisper_commentguids;
            IList<UploadFile> files = uploadFileList.Where(s => uploadFileGuids.Split(',').AsList().Contains(s.GUID)).ToList();
            IList<Comment> comments = commentList.Where(s => commentGuids.Split(',').AsList().Contains(s.GUID)).ToList();
            Whisper whisper = new Whisper(d.whisper_content, files, comments);
            Domain.Blog blog = new Domain.Blog(d.blog_account, (BlogType)d.blog_blogtype, whisper);
            return blog;
        }
        /// <summary>
        /// 将查询结果转为实体结果集
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private IList<Domain.Blog> Map(IEnumerable<dynamic> dynamics)
        {
            IList<Domain.Blog> blogs = new List<Domain.Blog>();
            #region 查询相关附件
            string sql = "SELECT * FROM UploadFile where uploadfile_guid in @Guids";
            List<string> guidList = new List<string>();
            dynamics.Select(s => s.whisper_uploadfileguids).ToList().ForEach(s =>
            {
                if (!string.IsNullOrEmpty(s))
                {
                    string guids = s;
                    guidList.AddRange(guids.Split(','));
                }
            });
            IEnumerable<dynamic> result = Select(sql, new { Guids = guidList });
            IList<UploadFile> uploadFileList = _uploadFileRepository.Map(result);
            #endregion

            #region 查询相关评论
            sql = "SELECT * FROM Comment where comment_guid in @Guids";
            guidList = new List<string>();
            dynamics.Select(s => s.whisper_commentguids).ToList().ForEach(s =>
            {
                if (!string.IsNullOrEmpty(s))
                {
                    string guids = s;
                    guidList.AddRange(guids.Split(','));
                }
            });
            result = Select(sql, new { Guids = guidList });
            IList<Comment> comments = _commentRepository.Map(result);
            #endregion
            foreach (var item in dynamics)
            {
                Domain.Blog blog = Map(item, uploadFileList, comments);
                blogs.Add(blog);
            }
            return blogs;
        }
        #endregion

        #region 
        /// <summary>
        /// 发表微语
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="blog"></param>
        /// <returns></returns>
        public void InsertWhisper(Domain.Blog blog)
        {
            useTransaction = true;
            CreateConnection(s => { return Insert(s, blog); });
        }

        /// <summary>
        /// 分页查询微语
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Domain.Blog> SelectByPage(int pageIndex, int pageSize)
        {
            int pageId = pageSize * (pageIndex - 1);
            string sql = "SELECT * FROM Blog INNER JOIN Whisper ON blog_blogbaseid=whisper_id " +
                " WHERE blog_id>" + pageId + " ORDER BY blog_id DESC  LIMIT  " + pageSize;
            IEnumerable<dynamic> dynamics = CreateConnection(s =>
            {
                return Select(s, sql);
            });
            return Map(dynamics);
        }
        #endregion

        #region 执行ado
        private dynamic Insert(IDbConnection dbConnection, Domain.Blog blog)
        {
            Whisper whisper = blog.BlogBase as Whisper;
            Whisper.SetUploadFileGuids(whisper);
            IList<UploadFile> uploadFiles = whisper.UploadFileList;
            //插入附件
            string insertFileSql = "INSERT INTO UploadFile(uploadfile_guid,uploadfile_useraccount, uploadfile_savepath, uploadfile_filename, uploadfile_filesize)" +
                " VALUES (@GUID, @Account, @SavePath, @FileName, @FileSize);";
            dbConnection.Execute(insertFileSql, uploadFiles, dbTransaction);
            //插入微语
            string insertWhisperSql = "INSERT INTO Whisper(whisper_content, whisper_commentguids, whisper_praisecount, whisper_praiseaccount, whisper_uploadfileguids, whisper_createtime)" +
                " VALUES ( @Content, @Commentguids, @PraiseCount, @PraiseAccount, @UploadfileGuids, NOW());SELECT @@identity";
            int whisperId = dbConnection.ExecuteScalar<int>(insertWhisperSql, whisper, dbTransaction);
            //插入实体
            Domain.Blog.SetBlogBaseId(whisperId, blog);
            string insertBlogSql = "INSERT INTO Blog(blog_account, blog_blogbaseid, blog_blogtype, blog_createtime) " +
                "VALUES (@Account, @BlogBaseId, @GetBlogType, NOW());";
            return dbConnection.Execute(insertBlogSql, blog, dbTransaction);
        }

        private IEnumerable<dynamic> Select(IDbConnection dbConnection, string sql, object obj = null)
        {
            return dbConnection.Query(sql, obj);
        }

        public int SelectCount()
        {
            string sql = "SELECT COUNT(*) FROM Blog";
            int recordCount = CreateConnection(s =>
            {
                return s.ExecuteScalar<int>(sql);
            });
            return recordCount;
        }
        #endregion
    }
}
