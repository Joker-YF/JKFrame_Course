using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKFrame;
using System;

[Serializable]
public class UserSetting
{
    public float GlobalVolume;
    public float BGVolume;
    public float EffectVolume;
    public LanguageType LanguageType;

    public UserSetting(float globalVolume, float bGVolume, float effectVolume, LanguageType languageType)
    {
        GlobalVolume = globalVolume;
        BGVolume = bGVolume;
        EffectVolume = effectVolume;
        LanguageType = languageType;
    }
}
