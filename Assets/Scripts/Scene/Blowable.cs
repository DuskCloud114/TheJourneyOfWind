using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blowable : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D boxCollider2D;
    private Vector2 direction;
    
    private bool IsOpen;
    
    private bool IsLeft;
    private bool isRight;
    private bool isUp;
    private bool isDown;

    private Dictionary<Wind, bool> windDict;
    private Dictionary<Wind, Vector2> windContributions;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError(gameObject.name + " 缺少 Animator 组件，请检查对应预制体是否挂载了 Animator 组件");

        boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D == null) Debug.LogError(gameObject.name + " 缺少 BoxCollider2D 组件，请检查对应预制体是否挂载了 BoxCollider2D 组件");

        windDict = new Dictionary<Wind, bool>();
        windContributions = new Dictionary<Wind, Vector2>();
    }
    

    void Start()
    {

    }

    void Update()
    {
        
    }

    void OnDisable()
    {
        foreach (Wind wind in windDict.Keys)
        {
            if (wind != null) wind.WindStateChanged -= OnWindStateChanged;
        }

        windDict.Clear();
        windContributions.Clear();
    }

    void OnDestroy()
    {
        foreach (Wind wind in windDict.Keys)
        {
            if (wind != null) wind.WindStateChanged -= OnWindStateChanged;
        }

        windDict.Clear();
        windContributions.Clear();
    }

    public void SyncAnim()
    {
        if (direction != Vector2.zero) IsOpen = true;
        else IsOpen = false;

        if (direction.x < 0)
        {
            IsLeft = true;
            isRight = false;
        }
        else if (direction.x > 0)
        {
            IsLeft = false;
            isRight = true;
        }
        else
        {
            IsLeft = false;
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

        animator.SetBool("IsOpen", IsOpen);
        animator.SetBool("IsLeft", IsLeft);
        animator.SetBool("IsRight", isRight);
        animator.SetBool("IsUp", isUp);
        animator.SetBool("IsDown", isDown);
    }

    public void EnterWind(Wind wind)
    {
        if (wind == null) return;

        if (!windDict.ContainsKey(wind))
        {
            windDict.Add(wind, true);
            windContributions[wind] = Vector2.zero;
            
            wind.WindStateChanged += OnWindStateChanged;

            if (wind.IsOpen) AddDirection(wind);
        }

    }

    public void ExitWind(Wind wind)
    {
        if (wind == null) return;

        if (windDict.ContainsKey(wind))
        {
            wind.WindStateChanged -= OnWindStateChanged;

            RemoveDirection(wind);
            windDict.Remove(wind);
        }

    }

    public void OnWindStateChanged(Wind wind, bool IsOpen)
    {
        if (wind == null) return;
        if (!windContributions.ContainsKey(wind)) return;

        if (IsOpen) AddDirection(wind);
        else RemoveDirection(wind);
    }

    void AddDirection(Wind wind)
    {
        if (wind == null) return;
        if (windContributions.ContainsKey(wind) && windContributions[wind] == Vector2.zero)
        {
            Vector2 windDirection = wind.GetWindDirection();
            windContributions[wind] = windDirection;

            direction += windDirection;
        }

        SyncAnim();
    }

    void RemoveDirection(Wind wind)
    {
        if (wind == null) return;
        if (windContributions.ContainsKey(wind) && windContributions[wind] != Vector2.zero)
        {
            direction -= windContributions[wind];
            windContributions[wind] = Vector2.zero;
        }

        SyncAnim();
    }
}
