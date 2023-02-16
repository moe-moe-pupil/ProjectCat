// --------------------------------------------------------------------------------------------------------------
// <copyright file="SmartTimer.cs" company="ProjectCat Technologies and contributors.">
// 此源代码的使用受 GNU AFFERO GENERAL PUBLIC LICENSE version 3 许可证的约束, 可以在以下链接找到该许可证.
// Use of this source code is governed by the GNU AGPLv3 license that can be found through the following link.
// https://github.com/moe-moe-pupil/ProjectCat/blob/main/LICENSE
// </copyright>
// --------------------------------------------------------------------------------------------------------------

namespace RDKitTools.Utils
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    /// <summary lang='Zh-CN'>
    /// 中心计时器.
    /// </summary>
    /// <typeparam name="T">计时器内部数组类.</typeparam>
    public class SmartTimer<T>
        where T : TimerAction, ITimerAction, new()
    {
        private static readonly List<T> s_actions = new();
        private bool _hasReset = true;

        public void AddAction(double startTime, string name, bool active = false)
        {
            s_actions.Add(new T { StartTime = startTime, CurrentTime = startTime, Name = name, Active = active, DefaultActive = active });
        }

        public void RemoveAction(string actionName)
        {
            if (this.TryGetActionByName(actionName, out T? result))
            {
                s_actions.Remove(result);
            }
        }

        public void ClearActions()
        {
            s_actions.Clear();
        }

        public bool TryGetActionByName(string actionName, [NotNullWhen(returnValue: true)] out T? actionResult)
        {
            foreach (ref T action in CollectionsMarshal.AsSpan<T>(s_actions))
            {
                if (action.Name.Equals(actionName))
                {
                    actionResult = action;
                    return true;
                }
            }

            actionResult = null;
            return false;
        }

        public bool IsActionActive(string actionName)
        {
            if (this.TryGetActionByName(actionName, out T? result))
            {
                return result.Active;
            }

            return false;
        }

        public bool IsActionTimeGone(string actionName)
        {
            if (this.TryGetActionByName(actionName, out T? result))
            {
                if (result.CurrentTime > 0)
                {
                    return false;
                }
            }

            return true;
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
            if (this.TryGetActionByName(actionName, out T? result))
            {
                result.Reset();
            }
        }

        public void ResetAllActions()
        {
            if (!this._hasReset)
            {
                foreach (T action in new List<T>(s_actions))
                {
                    action.CurrentTime = action.StartTime;
                }
            }
        }

        public void Update(double delta, bool removeTimeout = true)
        {
            this._hasReset = false;
            foreach (T action in new List<T>(s_actions))
            {
                if (action.Active)
                {
                    action.CurrentTime -= delta;
                    if (removeTimeout && action.CurrentTime <= 0)
                    {
                        s_actions.Remove(action);
                    }
                }
            }
        }

        public void Start(string actionName)
        {
            if (this.TryGetActionByName(actionName, out T? result))
            {
                result.Start();
            }
        }

        public void Stop(string actionName)
        {
            if (this.TryGetActionByName(actionName, out T? result))
            {
                result.Stop();
            }
        }

        public void Toggle(string actionName)
        {
            if (this.TryGetActionByName(actionName, out T? result))
            {
                result.Toggle();
            }
        }
    }
}
