using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKFrame;
using Sirenix.OdinInspector;
using System;

/// <summary>
/// 创建怪物的配置
/// </summary>
[Serializable]
public class CreateMonsterConfig
{
    [HorizontalGroup("Group")]
    [HideLabel()]
    public Monster_Config Monster_Config;

    [HorizontalGroup("Group")]
    [LabelText("生成的概率-随机数大于次数则生成该怪物")]
    public float Probability;
}

[CreateAssetMenu(fileName = "LV_Config",menuName ="配置/关卡配置")]
public class LV_Config : ConfigBase
{
    [LabelText("怪物的生成间隔")]
    public float CreatMonsterInterval;
    [LabelText("场景中最大的怪物数量")]
    public int MaxMonsterCountOnScene;
    [LabelText("怪物配置，刷新概率")]
    public CreateMonsterConfig[] CreateMonsterConfigs;
}
