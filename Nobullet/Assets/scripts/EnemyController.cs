using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Material deathMaterial;
    GameObject player;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent.destination = player.transform.position;
    }

    public void Injured() {
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        gameObject.transform.Find("MasterCube").GetComponent<Renderer>().material = deathMaterial;
        gameObject.tag = "DropBullet";
        gameObject.GetComponent<EnemyController>().enabled = false;
    }
}
