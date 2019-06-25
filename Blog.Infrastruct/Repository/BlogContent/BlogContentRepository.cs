//using Blog.Domain;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using Dapper;
//using System.Text;

//namespace Blog.Infrastruct
//{
//    public class BlogContentRepository : Repository<BlogContent, int>, IBlogContentRepository
//    {
//        public void PublishWhisper(BlogContent blogContent)
//        {
//            CreateConnection(s => { return Insert(s, blogContent); });
//        }
//        private dynamic Insert(IDbConnection dbConnection, BlogContent blogContent)
//        {
//            useTransaction = true;
//            Whisper whisper = blogContent.ContentBase as Whisper;
//            IList<UploadFile> uploadFiles = whisper.UploadFileList;
//            string insertFileSql = "INSERT INTO UploadFile(Id, Useraccount, Savepath, Filename, Filesize, Submitdate)" +
//                " VALUES (@Id, @Useraccount, @Savepath, @Filename, @Filesize, NOW());";
//            dbConnection.Execute(insertFileSql, uploadFiles, dbTransaction);

//        }
//    }
//}
