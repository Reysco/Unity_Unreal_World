using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static float move;

    [SerializeField] private float moveSpeed = 4f; //se eu deixar ela public, da para mudar durante o game a velocidade do player


    [SerializeField] private bool jumping;
    [SerializeField] private float jumpSpeed = 5f;

    [SerializeField] private float ghostJump;

    [SerializeField] private bool isGrounded;
    public Transform feetPosition;
    [SerializeField] private Vector2 sizeCapsule;
    [SerializeField] private float angleCapsule = 180f;
    public LayerMask whatIsGround;

    [SerializeField] private bool attackingBool;

    [SerializeField] private bool doubleJumpAllowed;
    [SerializeField] private bool BlockDoubleJump;

    public ParticleSystem dust;


    Rigidbody2D rb;
    SpriteRenderer sprite;
    Animator animationPlayer;

    public bool doubleAtk, lockAtk = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animationPlayer = GetComponent<Animator>();

        sizeCapsule = new Vector2(0.37f, 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        //Reconhecer o chão
        isGrounded = Physics2D.OverlapCapsule(feetPosition.position, sizeCapsule, CapsuleDirection2D.Horizontal, angleCapsule, whatIsGround);



        move = Input.GetAxisRaw("Horizontal");


        //Animação pulo duplo
        if(BlockDoubleJump)
        {
            animationPlayer.SetBool("Double-Jump", true);
        }
        else{
            animationPlayer.SetBool("Double-Jump", false);
        }




        //Input do ataque do personagem
        if (Input.GetButtonDown("Fire1") && lockAtk == false)
        {
            attackingBool = true;
            if (isGrounded)
            {
                if (move == 0)
                {
                    animationPlayer.SetBool("Attack-Idle", true);
                    animationPlayer.SetBool("Double-Attack-Idle", false);

                    animationPlayer.SetBool("Attack-Walk-1", false);
                    animationPlayer.SetBool("Attack-Jump", false);

                }


                else
                {
                    animationPlayer.SetBool("Attack-Idle", false);
                    animationPlayer.SetBool("Double-Attack-Idle", false);

                    animationPlayer.SetBool("Attack-Walk-1", true);
                    animationPlayer.SetBool("Attack-Jump", false);

                    animationPlayer.SetBool("Double-Attack", false);
                }
            }
            else
            {
                animationPlayer.SetBool("Attack-Idle", false);

                animationPlayer.SetBool("Attack-Walk-1", false);
                animationPlayer.SetBool("Attack-Jump", true);

                animationPlayer.SetBool("Double-Attack", false);
                animationPlayer.SetBool("Double-Attack-Idle", false);
            }

            if (doubleAtk == true)
            {
                if (move == 0)
                {
                    animationPlayer.SetBool("Double-Attack-Idle", true);
                    animationPlayer.SetBool("Attack-Idle", false);
                }
                else {
                animationPlayer.SetBool("Double-Attack", true);
                animationPlayer.SetBool("Attack-Walk-1", false);
            }
            }

        }

        /*if(attackingBool == true && isGrounded)
        {
            move = 0;
        }*/



        //Input do pulo do personagem
        if (Input.GetButtonDown("Jump") && ghostJump > 0)
        {
            jumping = true;

        }

        //Input double jump
        if (Input.GetButtonDown("Jump") && doubleJumpAllowed && BlockDoubleJump == false)
        {
            jumping = true;
            BlockDoubleJump = true;

        }
        if (isGrounded)
        {
            BlockDoubleJump = false;
        }
       
        //Inverter posição do personagem
        if (move < 0)
        {
            sprite.flipX = true;
        }
        else if (move > 0)
        {
            sprite.flipX = false;
        }

        //Criar fumaça quando velocidade diferente de 0
        if (move != 0)
        {
            CreateDust();
        }

        //Animação do personagem (walk-run)
        if (move != 0)
        {
            animationPlayer.SetBool("Walk-Run", true);
        }
        else
        {
            animationPlayer.SetBool("Walk-Run", false);
        }

        //Animação do personagem pulando
        if (isGrounded)
        {

            doubleJumpAllowed = false;
            
            ghostJump = 0.04f;

            animationPlayer.SetBool("Jump-V", false);
            animationPlayer.SetBool("Fall", false);
            animationPlayer.SetBool("Jump-H", false);
            animationPlayer.SetBool("Fall-2", false);


            if (rb.velocity.x != 0 && move != 0)
            {
                animationPlayer.SetBool("Walk-Run", true);
            }
            else
            {
                animationPlayer.SetBool("Walk-Run", false);
            }
        }
        else
        {
            doubleJumpAllowed = true;

            ghostJump -= Time.deltaTime;

            if(ghostJump <= 0)
            {
                ghostJump = 0;
            }

            if(rb.velocity.x == 0)
            {
                animationPlayer.SetBool("Walk-Run", false);

                if (rb.velocity.y > 0)
                {
                    animationPlayer.SetBool("Jump-V", true);
                    animationPlayer.SetBool("Fall", false);
                    animationPlayer.SetBool("Jump-H", false);
                    animationPlayer.SetBool("Fall-2", false);
                    
                }

                if (rb.velocity.y < 0)
                {
                    animationPlayer.SetBool("Jump-V", false);
                    animationPlayer.SetBool("Fall", true);
                    animationPlayer.SetBool("Jump-H", false);
                    animationPlayer.SetBool("Fall-2", false);
                    animationPlayer.SetBool("Double-Jump", false);
                }
            }
            else
            {

                if(rb.velocity.y > 0)
                {
                    animationPlayer.SetBool("Jump-V", false);
                    animationPlayer.SetBool("Fall", false);
                    animationPlayer.SetBool("Jump-H", true);
                    animationPlayer.SetBool("Fall-2", false);
                }

                if (rb.velocity.y < 0)
                {
                    animationPlayer.SetBool("Jump-V", false);
                    animationPlayer.SetBool("Fall", false);
                    animationPlayer.SetBool("Jump-H", false);
                    animationPlayer.SetBool("Fall-2", true);
                    animationPlayer.SetBool("Double-Jump", false);
                }
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(feetPosition.position, sizeCapsule);
    }


    void FixedUpdate()
    {
        //movimentação do personagem
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 5.5f;
        }
        else
        {
            moveSpeed = 4f;

        }
       

        //Pulo do personagem
            if (jumping)
        {
            CreateDust();

            rb.velocity = Vector2.up * jumpSpeed;

            //desativar pulo
            jumping = false;
        }

        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

    }

    void EndAnimationATK()
    {
        animationPlayer.SetBool("Attack-Walk-1", false);
        animationPlayer.SetBool("Attack-Jump", false);
        animationPlayer.SetBool("Attack-Idle", false);

        attackingBool = false;
    }

    void EndAnimationDoubleATK()
    {
        animationPlayer.SetBool("Double-Attack", false);
        animationPlayer.SetBool("Double-Attack-Idle", false);
        doubleAtk = false;
        attackingBool = false;
    }

    void CreateDust()
    {
        dust.Play();
    }


}