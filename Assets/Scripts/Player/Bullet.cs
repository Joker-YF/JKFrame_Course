using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKFrame;

[Pool]
public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rgb;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private new Collider collider;
    private int attack;

    public void Init(Vector3 dir,float movePower,int attack)
    {
        rgb.AddForce(dir.normalized * movePower);
        trailRenderer.emitting = true;
        collider.enabled = true;
        this.attack = attack;

        Invoke("DestoryOnInit", 20);
    }

    private void OnTriggerEnter(Collider other)
    {
        CancelInvoke("DestoryOnInit");
        StartCoroutine(Destory());
        // TODO:攻击AI
        if (other.gameObject.tag == "Monster")
        {
            other.gameObject.GetComponent<Monster_Controller>().GetHit(attack);
        }
    }

    private void DestoryOnInit()
    {
        StartCoroutine(Destory());
    }

    IEnumerator Destory()
    {
        collider.enabled = false;
        rgb.velocity = Vector3.zero;
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(2);
        // 销毁自身
        DoDestroy();
    }
    private void DoDestroy()
    {
        this.JKGameObjectPushPool();
    }
}
