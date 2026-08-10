using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        if (anim == null) Debug.LogError("玩家预制体身上缺少 Animator 组件，请检查预制体设置");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIsGrounded(bool isGrounded)
    {
        anim.SetBool("IsGrounded", isGrounded);
    }

    public void SetIsRunning(bool isRunning)
    {
        anim.SetBool("IsRunning", isRunning);
    }

    public void SetIsDashing(bool isDashing)
    {
        anim.SetBool("IsDashing", isDashing);
    }

    public void SetIsJumping(bool isJumping)
    {
        anim.SetBool("IsJumping", isJumping);
    }
}
