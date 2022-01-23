using JKFrame;
using System.Collections;
using UnityEngine;
public class Monster_Idle : Monster_StateBase
{
    private Coroutine goPatrolCoroutine;
    public override void Enter()
    {
        // 修改移动状态
        SetMoveState(false);
        // 播放动画
        PlayerAnimation("Idle");
        // 延迟一个随机的时间去巡逻
        goPatrolCoroutine = this.StartCoroutine(GoPatrol(Random.Range(1, 5f)));
    }
    private IEnumerator GoPatrol(float time)
    {
        yield return new WaitForSeconds(time);
        stateMachine.ChangeState<Monster_Patrol>((int)MonsterStateType.Patrol);
    }
    public override void Exit()
    {
        if (goPatrolCoroutine!=null)
        {
            this.StopCoroutine(goPatrolCoroutine);
            goPatrolCoroutine = null;
        }
    }


    public override void LateUpdate()
    {
        base.LateUpdate();
        // 检测和玩家的距离是否需要追击
        CheckFollowAndChangeState();
    }
}
