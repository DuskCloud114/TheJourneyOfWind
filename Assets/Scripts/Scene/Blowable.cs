using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blowable : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D boxCollider2D;
    private Vector2 direction;
    
    private bool isOpen;
    
    private bool isLeft;
    private bool isRight;
    private bool isUp;
    private bool isDown;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError(gameObject.name + " 缺少 Animator 组件，请检查对应预制体是否挂载了 Animator 组件");

        boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D == null) Debug.LogError(gameObject.name + " 缺少 BoxCollider2D 组件，请检查对应预制体是否挂载了 BoxCollider2D 组件");
    }
    

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void OnWindOpen(Wind wind, Vector2 dir)
    {
        direction += dir;
        if (direction != Vector2.zero) isOpen = true;
        else isOpen = false;

        if (direction.x < 0)
        {
            isLeft = true;
            isRight = false;
        }
        else if (direction.x > 0)
        {
            isLeft = false;
            isRight = true;
        }
        else
        {
            isLeft = false;
            isRight = false;
        }

        if (direction.y < 0)
        {
            isDown = true;
            isUp = false;
        }
        else if (direction.y > 0)
        {
            isDown = false;
            isUp = true;
        }
        else
        {
            isDown = false;
            isUp = false;
        }

        animator.SetBool("isOpen", isOpen);
        animator.SetBool("isLeft", isLeft);
        animator.SetBool("isRight", isRight);
        animator.SetBool("isUp", isUp);
        animator.SetBool("isDown", isDown);
    }
}
