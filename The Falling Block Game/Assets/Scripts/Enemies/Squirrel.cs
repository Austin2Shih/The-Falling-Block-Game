using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squirrel : MonoBehaviour
{
    public float acornCooldown;
    public float acornSpeed;
    private Vector3 acornVelocity;
    private float prevAcornToss;
    GameObject currAcorn;
    private int despawnHeight = -10; //distance from platform that bird will despawn at

    ObjectPooler objectPooler;
    EnemySpawner enemySpawner;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        enemySpawner = EnemySpawner.Instance;
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        processAcornAction();
        checkDespawn();
    }

    public void spawn(char coordLine, int moveDirection)
    {
        prepareAcorn();
        float xVel = 0;
        float zVel = 0;
        if (coordLine == 'x')
        {
            xVel = moveDirection * acornSpeed;
        }
        else if (coordLine == 'z')
        {
            zVel = moveDirection * acornSpeed;
        }
        acornVelocity = new Vector3(xVel, 0, zVel);
    }

    private void checkDespawn()
    {
        float currY = transform.position.y;
        if (currY < (despawnHeight * GameSettings.blockSize))
        {
            enemySpawner.despawnEnemy(this.gameObject, "Squirrel");
        }
    }

    private void processAcornAction()
    {
        if (Time.fixedTime - prevAcornToss >= acornCooldown)
        {
            Debug.Log(currAcorn.name);
            tossAcorn();
            prepareAcorn();
            prevAcornToss = Time.fixedTime;
        } 
    }

    private void prepareAcorn()
    {
        Vector3 spawnLocation = this.transform.position;
        GameObject spawnedAcorn = objectPooler.SpawnFromPool("Acorn", spawnLocation, Quaternion.identity);
        spawnedAcorn.GetComponent<Acorn>().initAcorn(acornVelocity);
        Debug.Log(spawnedAcorn.name);
        currAcorn = spawnedAcorn;
    }

    private void tossAcorn()
    {
        currAcorn.GetComponent<Acorn>().tossAcorn();
    }

    //Unity's built in on Collision for colliders set as Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            enemySpawner.despawnEnemy(this.gameObject, "Squirrel");
        }
    }
}
