using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private ObjectType type;
    public float speed = 20.0f;
    public Rigidbody2D rb;

    [SerializeField]
    private int damagePoint;

    [SerializeField]
    private float knockBack = 10.0f,
                    bulletFadeRadious = 20.0f, bulletFadeTime = 2.0f,
                    bulletDrag = 1.0f;

    private Vector2 playerPos;
    private Vector2 velocity;

    private Color colorInitial;
    private Color colorFinal;
    private SpriteRenderer spriteRenderer;
    private float t = 0.0f;


    void Start()
    {
        rb.velocity = transform.up * speed;
        velocity = rb.velocity;

        spriteRenderer = GetComponent<SpriteRenderer>();
        colorFinal = colorInitial = spriteRenderer.color;
        colorFinal.a = 0.0f;
    }

    private void Update()
    {
        playerPos = MainPlayerMovement.MainPlayer.transform.position;

        if (Vector2.Distance(playerPos, transform.position) > bulletFadeRadious)
        {
            Destroy(gameObject);
        }

        Color color = Color.Lerp(colorInitial, colorFinal, t);
        t += Time.deltaTime / bulletFadeTime;
        spriteRenderer.color = color;

        if (gameObject != null && color.a == 0.0f)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        velocity = velocity * (1 - Time.deltaTime * bulletDrag);
        rb.velocity = velocity;
    }
         
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {

        switch(type)
        {
            case ObjectType.MainPlayer:
                
                if(hitInfo.tag=="Enemy")
                {
                    EnemyAI enemy = hitInfo.GetComponent<EnemyAI>();
                    Vector2 dir = transform.position - enemy.transform.position;

                    if(enemy!=null)
                    {
                        enemy.OnTakeDamage(damagePoint, -dir, knockBack);
                        Destroy(gameObject);
                    }

                }

                break;

            case ObjectType.Enemy:

                if (hitInfo.tag == "MainPlayer")
                {
                    Vector2 dir = transform.position - MainPlayerMovement.MainPlayer.transform.position;

                    MainPlayerMovement.MainPlayer.PlayerKnockBack(damagePoint, (Vector3)dir, knockBack);
                    Destroy(gameObject);

                }

                break;

        }

    }

}
