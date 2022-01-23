using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using JKFrame;

/// <summary>
/// 怪物状态类型
/// </summary>
public enum MonsterStateType
{ 
    Idle,
    Patrol,
    Follow,
    Attack,
    GetHit,
    Die
}

[Pool]
public class Monster_Controller : MonoBehaviour,IStateMachineOwner
{
    #region 自身组件
    [SerializeField] private NavMeshAgent navMeshAgent;
    private StateMachine stateMachine;
    public NavMeshAgent NavMeshAgent { get => navMeshAgent; private set => navMeshAgent = value; }
    #endregion
    #region View组件
    private Monster_View view;
    public Animator animator { get; private set; }

    #endregion
    #region 数值
    private int hp;
    #endregion

    public void Init(Monster_Config config)
    {
        view = PoolManager.Instance.GetGameObject<Monster_View>(config.ModelPrefab, transform);
        view.transform.localPosition = Vector3.zero;
        view.Init(config.Attack);
        animator = view.Animator;

        hp = config.HP;

        // 初始化状态机
        stateMachine = ResManager.Load<StateMachine>();
        stateMachine.Init(this);
        stateMachine.ChangeState<Monster_Idle>((int)MonsterStateType.Idle);
    }

    /// <summary>
    /// 获取巡逻点
    /// </summary>
    public Vector3 GetPatrolTarget()
    {
        return Monster_Manager.Instance.GetPatrolTarget();
    }

    
    /// <summary>
    /// 被攻击
    /// </summary>
    public void GetHit(int damage)
    {
        if (hp == 0) return;
        hp -= damage;
        if (hp<=0)
        {
            hp = 0;
            stateMachine.ChangeState<Monster_Die>((int)MonsterStateType.Die);
        }
        else
        {
            stateMachine.ChangeState<Monster_GetHit>((int)MonsterStateType.GetHit);
        }
    }

    /// <summary>
    /// 逻辑调用的死亡销毁
    /// </summary>
    public void Die()
    {
        stateMachine.Destory();
        stateMachine = null;
        view.Destroy();
        view = null;
        this.JKGameObjectPushPool();
    }

    /// <summary>
    /// 因为场景切换导致的销毁，只能把非GameObject上的脚本放进对象池
    /// </summary>
    private void OnDestroy()
    {
        if (stateMachine!=null)
        {
            stateMachine.Destory();
        }
    }
}
