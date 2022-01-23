using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKFrame;
public class Monster_Attack : Monster_StateBase
{
    // 用于接收怪物View层的动画事件
    private int monsterEventID;
    public override void Init(IStateMachineOwner owner, int stateType, StateMachine stateMachine)
    {
        base.Init(owner, stateType, stateMachine);
        monsterEventID = monster.transform.GetInstanceID();
    }
    public override void Enter()
    {
        // 修改移动状态
        SetMoveState(false);
        // 播放动画
        PlayerAnimation("Attack");
        // 监听动画的攻击结束
        EventManager.AddEventListener("EndAttack_" + monsterEventID, OnAttackOver);
    }
    public override void Exit()
    {
        EventManager.RemoveEventListener("EndAttack_" + monsterEventID, OnAttackOver);
    }

    /// <summary>
    /// 当攻击结束时候执行的逻辑
    /// </summary>
    private void OnAttackOver()
    {
        stateMachine.ChangeState<Monster_Follow>((int)MonsterStateType.Follow);
    }
}
