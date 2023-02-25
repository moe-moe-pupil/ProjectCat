using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDKitTools.Utils
{
    public interface ITimerAction
    {
        public float StartTime { get; set; }

        public float CurrentTime { get; set; }

        public string Name { get; set; }
    }

    /// <summary lang='Zh-CN'>
    /// 基础计时器行为.
    /// </summary>
    public class TimerAction : ITimerAction
    {
        public TimerAction(float startTime, string name)
        {
            StartTime = startTime;
            CurrentTime = startTime;
            Name = name;
        }

        /// <summary lang='Zh-CN'>
        /// 生成时间.
        /// </summary>
        public float StartTime { get ; set; }

        public float CurrentTime { get ; set; }

        public string Name { get; set; }


    }
}
