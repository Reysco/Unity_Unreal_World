using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleStone : MonoBehaviour
{


    [SerializeField] private int lifeStone = 3;

        public SpriteRenderer stone;
    public Sprite[] stoneImages = new Sprite[3];

    void Start()
    {
        stone = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        switch (lifeStone)
        {
            case 3:
                stone.sprite = stoneImages[2];
                break;
            case 2:
                stone.sprite = stoneImages[1];
                break;
            case 1:
                stone.sprite = stoneImages[0];
                break;
            case 0:
                GetComponent<Animator>().enabled = true;
                Destroy(GetComponent<Animator>(), 1);

                Destroy(GetComponent<BoxCollider2D>());
                Destroy(GetComponent<SimpleStone>());

                break;
        }
    }

    void OnTriggerEnter2D(Collider2D Cool)
    {
        if (Cool.gameObject.tag == "PlayerAttack")
        {
            lifeStone--;
        }
    }
}
