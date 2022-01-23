using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKFrame;

public class Camera_Controller : SingletonMono<Camera_Controller>
{
    private Player_Controller player;
    public new Camera camera { get; private set; }
    private Vector3 offset = new Vector3(0,6.5f,-5f);
    private float speed = 2;
    void Start()
    {
        player = Player_Controller.Instance;
    }
    protected override void Awake()
    {
        base.Awake();
        camera = GetComponent<Camera>();
    }
    private void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset,Time.deltaTime);
    }
}
