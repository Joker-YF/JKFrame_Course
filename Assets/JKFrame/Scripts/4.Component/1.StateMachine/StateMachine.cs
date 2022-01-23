﻿using System.Collections.Generic;
namespace JKFrame
{
    public interface IStateMachineOwner { }

    /// <summary>
    /// 状态机控制器
    /// </summary>
    [Pool]
    public class StateMachine
    {
        // 当前状态
        public int CurrStateType { get; private set; } = -1;

        // 当前生效中的状态
        private StateBase currStateObj;

        // 宿主
        private IStateMachineOwner owner;

        // 所有的状态 Key:状态枚举的值 Value:具体的状态
        private Dictionary<int, StateBase> stateDic = new Dictionary<int, StateBase>();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="owner">宿主</param>
        public void Init(IStateMachineOwner owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <typeparam name="T">具体要切换到的状态脚本类型</typeparam>
        /// <param name="newState">新状态</param>
        /// <param name="reCurrstate">新状态和当前状态一致的情况下，是否也要切换</param>
        /// <returns></returns>
        public bool ChangeState<T>(int newStateType, bool reCurrstate = false) where T : StateBase, new()
        {
            // 状态一致，并且不需要刷新状态，则切换失败
            if (newStateType == CurrStateType && !reCurrstate) return false;

            // 退出当前状态
            if (currStateObj != null)
            {
                currStateObj.Exit();
                currStateObj.RemoveUpdate(currStateObj.Update);
                currStateObj.RemoveLateUpdate(currStateObj.LateUpdate);
                currStateObj.RemoveFixedUpdate(currStateObj.FixedUpdate);
            }
            // 进入新状态
            currStateObj = GetState<T>(newStateType);
            CurrStateType = newStateType;
            currStateObj.Enter();
            currStateObj.OnUpdate(currStateObj.Update);
            currStateObj.OnLateUpdate(currStateObj.LateUpdate);
            currStateObj.OnFixedUpdate(currStateObj.FixedUpdate);

            return true;
        }

        /// <summary>
        /// 从对象池获取一个状态
        /// </summary>
        private StateBase GetState<T>(int stateType) where T : StateBase, new()
        {
            if (stateDic.ContainsKey(stateType)) return stateDic[stateType];

            StateBase state = ResManager.Load<T>();
            state.Init(owner, stateType, this);
            stateDic.Add(stateType, state);
            return state;
        }

        /// <summary>
        /// 停止工作
        /// 把所有状态都释放，但是StateMachine未来还可以工作
        /// </summary>
        public void Stop()
        {
            // 处理当前状态的额外逻辑
            currStateObj.Exit();
            currStateObj.RemoveUpdate(currStateObj.Update);
            currStateObj.RemoveLateUpdate(currStateObj.LateUpdate);
            currStateObj.RemoveFixedUpdate(currStateObj.FixedUpdate);
            CurrStateType = -1;
            currStateObj = null;
            // 处理缓存中所有状态的逻辑
            var enumerator = stateDic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                enumerator.Current.Value.UnInit();
            }
            stateDic.Clear();
        }

        /// <summary>
        /// 销毁，宿主应该释放掉StateMachin的引用
        /// </summary>
        public void Destory()
        {
            // 处理所有状态
            Stop();
            // 放弃所有资源的引用
            owner = null;

            // 放进对象池
            this.JKObjectPushPool();
        }
    }
}