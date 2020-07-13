using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data;

namespace Blog.Infrastruct
{
    public class VideoRepository : Repository, IVideoRepository
    {
        private Video Map(dynamic d)
        {
            return new Video(d.video_id,
                d.video_description, 
                d.video_author,
                d.video_url, 
                d.video_lable,
                d.video_length,
                d.video_watch_count,
                d.video_createtime);
        }
        private IList<Video> Map(IEnumerable<dynamic> list)
        {
            IList<Video> videos = new List<Video>();
            foreach(var item in list)
            {
                videos.Add(Map(item));
            }
            return videos;
        }
        private string Where(VideoCondition condition, ref DynamicParameters parameters)
        {
            IList<string> sqlList = new List<string>();
            if(condition==null)
                   return " 1=1 ";
            sqlList.Add(" 1=1 ");
            string sql = string.Join(" AND ", sqlList);
            return sql;
        }
        public void Insert(Video video)
        {
            string sql = "INSERT INTO T_Video(video_author,video_url,video_description,video_lable,video_length,video_watch_count,video_createtime) " +
                 "VALUES(@Author,@Url,@Description,@Lable,@Length,@WatchCount,NOW())";
            DbConnection.Execute(sql,video);
        }
        public Video SelectById(int id)
        {
            string sql = "SELECT * FROM T_Video WHERE video_id=@Id";
            dynamic result = base.SelectSingle(sql,new {Id=id });
            return Map(result);
        }

        public IList<Video> SelectByPage(int pageSize, int pageIndex, VideoCondition condition = null)
        {
            int pageId = pageSize * (pageIndex - 1);
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("pageId", pageId, DbType.Int32);
            parameters.Add("pageSize", pageSize, DbType.Int32);
            string where = Where(condition,ref parameters);
            string sql = "SELECT * FROM T_Video WHERE "+where+ " ORDER BY video_createtime limit @pageId,@pageSize";
            IEnumerable<dynamic> list = DbConnection.Query(sql, parameters);
            return Map(list);
        }

        public int SelectCount(VideoCondition videoCondition = null)
        {
            DynamicParameters parameters = new DynamicParameters();
            string where = Where(videoCondition, ref parameters);
            string sql = "SELECT COUNT(*) FROM T_Video WHERE " + where;
            return base.SelectCount(sql, parameters);
        }
    }
}
