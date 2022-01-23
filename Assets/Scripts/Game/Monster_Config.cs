using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKFrame;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(fileName = "Monster_Config", menuName = "配置/怪物配置")]
public class Monster_Config : ConfigBase
{
    public int HP;
    public int Attack;
    public GameObject ModelPrefab;
}
