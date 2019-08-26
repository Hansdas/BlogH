//using Blog.Domain;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Blog.Infrastruct
//{
//    public class UploadFileRepository : Repository<UploadFile, int>, IUploadFileRepository
//    {
//        /// <summary>
//        /// 将查询结果转为实体
//        /// </summary>
//        /// <param name="d"></param>
//        /// <returns></returns>
//        public UploadFile Map(dynamic d)
//        {
//            return new UploadFile(
//                  d.uploadfile_useraccount
//                , d.uploadfile_guid
//                , d.uploadfile_savepath
//                , d.uploadfile_filename
//                , d.uploadfile_filesize);
//        }
//        /// <summary>
//        /// 将查询结果转为实体结果集
//        /// </summary>
//        /// <param name="d"></param>
//        /// <returns></returns>
//        public IList<UploadFile> Map(IEnumerable<dynamic> dynamics)
//        {
//            IList<UploadFile> uploadFiles = new List<UploadFile>();
//            foreach (var d in dynamics)
//            {
//                UploadFile uploadFile = Map(d);
//                uploadFiles.Add(uploadFile);
//            }
//            return uploadFiles;
//        }
//    }
//}
