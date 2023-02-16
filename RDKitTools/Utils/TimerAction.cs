// --------------------------------------------------------------------------------------------------------------
// <copyright file="TimerAction.cs" company="ProjectCat Technologies and contributors.">
// 此源代码的使用受 GNU AFFERO GENERAL PUBLIC LICENSE version 3 许可证的约束, 可以在以下链接找到该许可证.
// Use of this source code is governed by the GNU AGPLv3 license that can be found through the following link.
// https://github.com/moe-moe-pupil/ProjectCat/blob/main/LICENSE
// </copyright>
// --------------------------------------------------------------------------------------------------------------

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
            this.StartTime = startTime;
            this.CurrentTime = startTime;
            this.Name = name;
        }

        /// <summary lang='Zh-CN'>
        /// Gets or sets 生成时间.
        /// </summary>
        public float StartTime { get; set; }

        public float CurrentTime { get; set; }

        public string Name { get; set; }
    }
}
