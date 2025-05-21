using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    //Aqui reconhece o atack do player

    private CapsuleCollider2D colliderAtkPlayer;



    void Start()
    {
        colliderAtkPlayer = GetComponent<CapsuleCollider2D>();
    }

    
    void Update()
    {
        //Inverter posição do colisor do ataque com base onde ele olha
        if(PlayerMovement.move < 0)
        {
            colliderAtkPlayer.offset = new Vector2(-0.6f, 0);
        }else if(PlayerMovement.move > 0)
        {
            colliderAtkPlayer.offset = new Vector2(0.6f, 0);
        }
    }
}
