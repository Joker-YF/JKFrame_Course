using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKFrame;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName ="配置/玩家配置",fileName ="Player_Config")]
public class Player_Config : ConfigBase
{
    [LabelText("移动速度")]
    public float MoveSpeed = 2;
    [LabelText("生命值")]
    public int HP = 100;
    [LabelText("最大子弹数量")]
    public int MaxBulletNum = 30;
    [LabelText("射击间隔")]
    public float ShootInterval = 0.02f;
    [LabelText("子弹移动力量")]
    public float BulletMovePower= 1000;
    [LabelText("攻击力")]
    public int Attack = 20;
}
