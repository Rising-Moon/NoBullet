using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public float speed;
    public LayerMask clickableLayer;
    public GameObject particle;
    public GameObject collector;
    public float bulletSpeed;
    public float bulletLifeTime;
    public Slider hpSlider;

    bool isSbsorb;
    GameObject bullet;
    bool haveBullet;
    int hp;
    ParticleSystem ps;

    // Start is called before the first frame update
    void Start() {
        particle.GetComponent<BoxCollider>().enabled = false;
        ps = particle.GetComponent<ParticleSystem>();
        ps.Stop();
        bullet = null;
        isSbsorb = false;
        haveBullet = false;
        hp = 100;
    }

    void Update() {
        changeGUI();

        RaycastHit hit;
        Ray MouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(MouseRay, out hit, 50, clickableLayer.value)) {
            LookAtMouse(hit.point);
        }
        Motion(hit.point);

    }

    // Update is called once per frame
    void FixedUpdate() {
        Move();
    }

    // Trigger stay
    void OnTriggerStay(Collider other) {

    }

    public void sbsorbCollider(Collider other) {
        if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("DropBullet")) {
            if (isSbsorb) {
                GameObject into = other.gameObject;
                into.transform.parent = gameObject.transform;
                bullet = into;
                originalScale = into.transform.localScale;
                InvokeRepeating("Collect", 0f, 0.01f);
            }
        }
    }

    public void hurtCollider(Collider other) {
        if (other.gameObject.CompareTag("Bullet")) {
            Hurt(30);
            Destroy(other.gameObject);
        }
    }

    Vector3 originalScale;
    void Collect() {
        Vector3 targetPos = collector.transform.position;
        bullet.transform.localScale = bullet.transform.localScale * 95 / 100;
        bullet.transform.position = (bullet.transform.position - targetPos) * 95 / 100 + targetPos;
        if (Vector3.Distance(bullet.transform.position, targetPos) < 0.3f) {
            gameObject.GetComponents<AudioSource>()[1].Play();
            CancelInvoke("Collect");
            haveBullet = true;
        }
    }

    void Move() {
        float AxisX = Input.GetAxis("Horizontal");
        float AxisZ = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(0, 0, 0);

        if ((Math.Abs(AxisX) + Math.Abs(AxisZ)) != 0) {
            float moveX = Math.Abs(AxisX) / (Math.Abs(AxisX) + Math.Abs(AxisZ)) * AxisX;
            float moveY = Math.Abs(AxisZ) / (Math.Abs(AxisX) + Math.Abs(AxisZ)) * AxisZ;
            move = new Vector3(moveX, 0, moveY);
        }
        transform.position = transform.position + (move * speed * Time.deltaTime);

    }

    void LookAtMouse(Vector3 point) {
        Vector3 lookPos = point;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);
    }

    void Motion(Vector3 hitPoint) {
        //shot
        if (Input.GetMouseButtonDown(0) && haveBullet) {
            hitPoint.y = 0;
            bullet.transform.parent = gameObject.transform.parent;
            bullet.transform.localScale = originalScale;
            bullet.tag = "FlyingBullet";
            bullet.GetComponent<BoxCollider>().isTrigger = true;
            bullet.AddComponent<BulletController>();
            bullet.GetComponent<BulletController>().initBullet(hitPoint, bulletSpeed, bulletLifeTime);            
            bullet = null;
            haveBullet = false;
            gameObject.GetComponents<AudioSource>()[2].Play();
        }
        //sbsorb
        if (Input.GetMouseButtonDown(1) && ps.isStopped && !haveBullet) {
            particle.GetComponent<BoxCollider>().enabled = true;
            ps.Play();
            isSbsorb = true;
            gameObject.GetComponents<AudioSource>()[0].Play();
            Invoke("Stop", 2f);
        }
        //test
        if (Input.GetMouseButtonDown(2)) {
            Debug.Log("Mouse 3 down");
            Hurt(30);
        }
    }

    void Stop() {
        ps.Stop();
        isSbsorb = false;
        particle.GetComponent<BoxCollider>().enabled = false;
    }

    void Hurt(int value) {
        gameObject.GetComponents<AudioSource>()[3].Play();
        hp -= value;
        if (hp <= 0) {
            hp = 0;
            PlayerDeath();
        }
    }

    void PlayerDeath() {        
        GameObject deathBody = (GameObject)Resources.Load("Prefabs/DeathPlayer");
        deathBody = Instantiate(deathBody);
        deathBody.name = "DeathPlayer";
        deathBody.transform.position = gameObject.transform.position;
        deathBody.transform.rotation = gameObject.transform.rotation;
        Destroy(gameObject);
    }

    void changeGUI() {
        if (hp != 0)
            hpSlider.value = (float)hp / 100;
        else {
            hpSlider.enabled = false;
        }
    }
}
