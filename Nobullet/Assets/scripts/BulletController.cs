using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    Vector3 target;
    float bulletSpeed;
    float bulletLifeTime;
    float distanceX;
    float distanceZ;
    float speedX;
    float speedZ;

    // Start is called before the first frame update
    void Start() {
        target.y = 0;
        distanceX = target.x - gameObject.transform.position.x;
        distanceZ = target.z - gameObject.transform.position.z;
        speedX = distanceX / (Mathf.Abs(distanceX) + Mathf.Abs(distanceZ)) * bulletSpeed;
        speedZ = distanceZ / (Mathf.Abs(distanceX) + Mathf.Abs(distanceZ)) * bulletSpeed;       
        Invoke("DistroyBullet", bulletLifeTime);
    }

    // Update is called once per frame
    void FixedUpdate() {
        Vector3 targetPoint = gameObject.transform.position;
        targetPoint.x += speedX * Time.deltaTime;
        targetPoint.z += speedZ * Time.deltaTime;
        gameObject.transform.position = targetPoint;
        transform.Rotate(new Vector3(75, 90, 105)*Time.deltaTime);
    }

    //Enter Trigger
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Enemy")) {
            other.gameObject.GetComponent<EnemyController>().Injured();
            CancelInvoke("DistroyBullet");
            DistroyBullet();
        }
    }

    public void initBullet(Vector3 target,float bulletSpeed,float bulletLifeTime) {
        gameObject.GetComponent<BulletController>().enabled = false;
        this.target = target;
        this.bulletSpeed = bulletSpeed;
        this.bulletLifeTime = bulletLifeTime;
        gameObject.GetComponent<BulletController>().enabled = true;
    }

    void DistroyBullet() {
        Destroy(gameObject);
    }
}
