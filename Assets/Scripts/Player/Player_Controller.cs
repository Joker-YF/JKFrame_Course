using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKFrame;

public enum PlayerState
{ 
    Normal,
    ReLoad,
    GetHit,
    Die
}
public class Player_Controller : SingletonMono<Player_Controller>
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator; 
    [SerializeField] private Transform firePoint; 
    private PlayerState playerState;
    
    #region 参数
    private float moveSpeed;
    private int currBulletNum;
    private int maxBulletNum;
    private float shootInterval;
    private float bulletMovePower;
    private int attack;
    private bool canShoot = true;
    private int hp;
    public int HP { get => hp;
        set {
            hp = value;
            // 更新血条
            EventManager.EventTrigger<int>("UpdateHP", hp);
        }
    }
    #endregion
    private int groundLayerMask;
    public PlayerState PlayerState
    {
        get => playerState;
        set
        {
            playerState = value;
            switch (playerState)
            { 
                case PlayerState.ReLoad:
                    StartCoroutine(ReLoad());
                    break;
                case PlayerState.GetHit:
                    // 重置上一次受伤带来的效果
                    StopCoroutine(DoGetHit());
                    animator.SetBool("GetHit", false);

                    // 开始这一次受伤带来的效果
                    animator.SetBool("GetHit", true);
                    StartCoroutine(DoGetHit());
                    break;
                case PlayerState.Die:
                    EventManager.EventTrigger("GameOver");
                    animator.SetTrigger("Die");
                    break;
            }
        }
    }



    public void Init(Player_Config config)
    {
        HP = config.HP;
        moveSpeed = config.MoveSpeed;
        maxBulletNum = config.MaxBulletNum;
        currBulletNum = maxBulletNum;
        shootInterval = config.ShootInterval;
        bulletMovePower = config.BulletMovePower;
        attack = config.Attack;
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            StateOnUpdate();
        }
    }
    private void StateOnUpdate()
    {
        switch (PlayerState)
        {
            case PlayerState.Normal:
                Move();
                Shoot();
                if (currBulletNum<maxBulletNum&&Input.GetKeyDown(KeyCode.R))
                {
                    PlayerState = PlayerState.ReLoad;
                }
                break;
            case PlayerState.ReLoad:
                Move();
                break;
        }
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(h, -5, v);
        characterController.Move(moveDir * moveSpeed * Time.deltaTime);

        Ray ray = Camera_Controller.Instance.camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray,out RaycastHit hitInfo,1000, groundLayerMask))
        {
            if (hitInfo.point.z<transform.position.z)
            {
                v *= -1;
                h *= -1;
            }
            Vector3 dir = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z) - transform.position;
            Quaternion targetQuaternion = Quaternion.FromToRotation(Vector3.forward, dir);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetQuaternion, Time.deltaTime * 20f);
        }

        animator.SetFloat("MoveX", h);
        animator.SetFloat("MoveY", v);
    }

    private void Shoot()
    {
        if (canShoot&&currBulletNum>0&&Input.GetMouseButton(0))
        {
            StartCoroutine(DoShoot());
        }
        else
        {
            animator.SetBool("Shoot", false);
        }
    }

    private IEnumerator DoShoot()
    {
        currBulletNum -= 1;
        // 修改UI
        EventManager.EventTrigger<int, int>("UpdateBullet", currBulletNum, maxBulletNum);
        animator.SetBool("Shoot", true);
        canShoot = false;
        AudioManager.Instance.PlayOnShot("Audio/Shoot/laser_01", transform.position);
        // 生成子弹
        Bullet bullet = ResManager.Load<Bullet>("Bullet",LVManager.Instance.TempObjRoot);
        bullet.transform.position = firePoint.position;
        bullet.Init(firePoint.forward,bulletMovePower,attack);
        yield return new WaitForSeconds(shootInterval);
        canShoot = true;
        // 子弹打完，需要换弹
        if (currBulletNum ==0)
        {
            PlayerState = PlayerState.ReLoad;
        }
    }

    private IEnumerator ReLoad()
    {
        animator.SetBool("ReLoad", true);
        AudioManager.Instance.PlayOnShot("Audio/Shoot/ReLoad", this);
        yield return new WaitForSeconds(1.9f);
        animator.SetBool("ReLoad", false);
        PlayerState = PlayerState.Normal;
        currBulletNum = maxBulletNum;
        EventManager.EventTrigger<int, int>("UpdateBullet", currBulletNum, maxBulletNum);
    }

    public void GetHit(int damage)
    {
        if (hp == 0) return;
        hp -= damage;
        if (hp<=0)
        {
            HP = 0;
            PlayerState = PlayerState.Die;
        }
        else
        {
            HP = hp;
            PlayerState = PlayerState.GetHit;
        }
    }

    private IEnumerator DoGetHit()
    {
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("GetHit", false);
        if (PlayerState==PlayerState.GetHit)
        {
            PlayerState = PlayerState.Normal;
        }
    }
}
