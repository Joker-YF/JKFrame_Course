using System.Collections;
using UnityEngine;
using JKFrame;

public class Monster_GetHit : Monster_StateBase
{
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
        PlayerAnimation("GetHit");
        // 监听动画的攻击结束
        EventManager.AddEventListener("EndGetHit_" + monsterEventID, OnEndGetHit);
    }
    public override void Exit()
    {
        EventManager.RemoveEventListener("EndGetHit_" + monsterEventID, OnEndGetHit);
    }
    private void OnEndGetHit()
    {
        stateMachine.ChangeState<Monster_Follow>((int)MonsterStateType.Follow);
    }

}
