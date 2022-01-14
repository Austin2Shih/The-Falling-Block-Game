using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acorn : MonoBehaviour
{

    public int despawnDistance;
    private Vector3 acornVelocity;
    EnemySpawner enemySpawner;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = EnemySpawner.Instance;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        checkDespawn();
    }

    public void initAcorn(Vector3 acornVelocity)
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        this.acornVelocity = acornVelocity;
    }

    public void tossAcorn()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = acornVelocity;
    }

    private void checkDespawn()
    {
        float currX = transform.position.x;
        float currZ = transform.position.z;
        if (currX < (-despawnDistance * GameSettings.blockSize) || currZ < (-despawnDistance * GameSettings.blockSize)
            || currX > (GameSettings.gridX + despawnDistance) * GameSettings.blockSize
            || currZ > (GameSettings.gridZ + despawnDistance) * GameSettings.blockSize)
        {
            enemySpawner.despawnEnemy(this.gameObject, "Acorn");
        }
    }
}
