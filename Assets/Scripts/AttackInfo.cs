using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInfo
{
    public AttackInfo(int _dmg, float _knockback){
        dmg = _dmg;
        knockback = _knockback;
    }

    public int dmg;
    public float knockback;
}
