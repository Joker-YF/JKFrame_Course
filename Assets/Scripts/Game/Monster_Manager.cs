using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKFrame;
/// <summary>
/// 怪物管理器
/// </summary>
public class Monster_Manager : LogicManagerBase<Monster_Manager>
{
    [SerializeField] private Transform creatMonsterPoint;
    [SerializeField] private Transform[] targets;
    private int monsterCount = 0;
    private LV_Config config;

    private void Start()
    {
        config = ConfigManager.Instance.GetConfig<LV_Config>("LV");
        InvokeRepeating("CreateMonster", config.CreatMonsterInterval, 1);
    }
    protected override void RegisterEventListener()
    {
        EventManager.AddEventListener("MonsterDie", OnMonsterDie);
    }
    protected override void CancelEventListener()
    {
        EventManager.RemoveEventListener("MonsterDie", OnMonsterDie);
    }

    private void OnMonsterDie()
    {
        monsterCount -= 1;
    }

    // 每X秒生成一只怪物
    private void CreateMonster()
    {
        // 当前未达到怪物上限
        if (monsterCount<config.MaxMonsterCountOnScene)
        {
            float randomNum = Random.value;
            monsterCount += 1;
            for (int i = 0; i < config.CreateMonsterConfigs.Length; i++)
            {
                // 当前随机数大于配置中的概率
                if (randomNum>config.CreateMonsterConfigs[i].Probability)
                {
                    Monster_Controller monste = ResManager.Load<Monster_Controller>("Monster", LVManager.Instance.TempObjRoot);
                    monste.transform.position = creatMonsterPoint.position;
                    monste.Init(config.CreateMonsterConfigs[i].Monster_Config);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// 获取巡逻点
    /// </summary>
    public Vector3 GetPatrolTarget()
    {
        return targets[Random.Range(0, targets.Length)].position;
    }


}
