// --------------------------------------------------------------------------------------------------------------
// <copyright file="SmartTimer.cs" company="ProjectCat Technologies and contributors.">
// 此源代码的使用受 GNU AFFERO GENERAL PUBLIC LICENSE version 3 许可证的约束, 可以在以下链接找到该许可证.
// Use of this source code is governed by the GNU AGPLv3 license that can be found through the following link.
// https://github.com/moe-moe-pupil/ProjectCat/blob/main/LICENSE
// </copyright>
// --------------------------------------------------------------------------------------------------------------

namespace RDKitTools.Utils
{
    /// <summary lang='Zh-CN'>
    /// 中心计时器.
    /// </summary>
    /// <typeparam name="T">计时器内部数组类.</typeparam>
    public class SmartTimer<T>
        where T : TimerAction, ITimerAction, new()
    {
        private static readonly List<T> s_actions = new ();

        public void AddAction(float startTime, string name)
        {
            s_actions.Add(new T { StartTime = startTime, CurrentTime = startTime, Name = name });
        }

        public void RemoveAction(string actionName)
        {
            foreach (T action in new List<T>(s_actions))
            {
                if (action.Name.Equals(actionName))
                {
                    s_actions.Remove(action);
                }
            }
        }

        public void ClearActions()
        {
            s_actions.Clear();
        }

        public bool ContainsAction(string actionName)
        {
            foreach (T action in new List<T>(s_actions))
            {
                if (action.Name.Equals(actionName))
                {
                    return true;
                }
            }

            return false;
        }

        public void ResetAction(string actionName)
        {
            foreach (T action in new List<T>(s_actions))
            {
                if (action.Name.Equals(actionName))
                {
                    action.CurrentTime = action.StartTime;
                }
            }
        }

        public void ResetAllActions()
        {
            foreach (T action in new List<T>(s_actions))
            {
                action.CurrentTime = action.StartTime;
            }
        }

        public void Update(float delta, bool removeTimeout = true)
        {
            foreach (T action in new List<T>(s_actions))
            {
                action.CurrentTime -= delta;
                if (removeTimeout && action.CurrentTime <= 0)
                {
                    s_actions.Remove(action);
                }
            }
        }
    }
}
