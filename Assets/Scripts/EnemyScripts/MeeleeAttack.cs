using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleeAttack : Attack
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Perform(GameObject player, Enemy enemy) {
        //TODO: play animation & sound
        //player.GetAttacked(enemy.getDamage());
    }
}
