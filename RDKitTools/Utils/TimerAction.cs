using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDKitTools.Utils
{
    public interface ITimerAction
    {
        public double StartTime { get; set; }

        public double CurrentTime { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public bool DefaultActive { get; set; }
    }

    /// <summary lang='Zh-CN'>
    /// 基础计时器行为.
    /// </summary>
    public class TimerAction : ITimerAction
    {
        public TimerAction()
        {
        }

        /// <summary lang='Zh-CN'>
        /// 生成时间.
        /// </summary>

        public double StartTime { get; set; }

        public double CurrentTime { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public bool DefaultActive { get; set; }

        public void Reset()
        {
            this.CurrentTime = this.StartTime;
            this.Active = this.DefaultActive;
        }

        public void Start()
        {
            this.Active = true;
        }

        public void Stop()
        {
            this.Active = false;
        }

        public void Toggle()
        {
            this.Active = !this.Active;
        }

    }
}
