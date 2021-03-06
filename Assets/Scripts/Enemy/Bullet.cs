using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100;
    Transform src, dst;
    Vector3 dir;

    void Start()
    {
        Destroy(gameObject, 1);
        src = this.GetComponentInParent<Transform>();
        dst = GameObject.Find("Player").transform;
        dir = dst.position - src.position;
        dir += new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
    }

    void Update()
    {
        transform.position += speed * dir.normalized * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }
}
