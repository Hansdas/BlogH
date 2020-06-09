﻿using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Quartz
{
   public class QuartzApplication 
    {
        private  IScheduler _scheduler;
        private  ISchedulerFactory _schedulerFactory;
        public QuartzApplication()
        {

        }
        public QuartzApplication(ISchedulerFactory schedulerFactory)
        {
           _schedulerFactory = schedulerFactory;
        }
        public async Task StartJob()
        {
            _schedulerFactory = new StdSchedulerFactory();
            _scheduler = await _schedulerFactory.GetScheduler();
            await _scheduler.Start();
            await CreateJon<NewsQuartz>("news","news", "0 0 1 * * ? *");
        }
        private async Task CreateJon<T>(string name, string group, string cron) where T:IJob
        {
            //创建定时器
            ITrigger trigger = TriggerBuilder.Create().WithIdentity(name, group).WithCronSchedule(cron).Build();
            //创建任务作业
            IJobDetail job = JobBuilder.Create<T>().WithIdentity(name, group).Build();
            await _scheduler.ScheduleJob(job, trigger);
        }
    }
}
