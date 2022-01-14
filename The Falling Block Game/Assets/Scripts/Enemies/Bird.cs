using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float moveSpeed;

    private int despawnDistance = 5; //distance from platform that bird will despawn at

    EnemySpawner enemySpawner;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = EnemySpawner.Instance;
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        checkDespawn();
    }

    public void spawn(char coordLine, int moveDirection)
    {
        rb = this.GetComponent<Rigidbody>();
        float xVel = 0;
        float zVel = 0;
        if (coordLine == 'x')
        {
            transform.rotation = Quaternion.Euler(new Vector3(90, 90, 0));
            xVel = moveDirection * moveSpeed;
        } else if (coordLine == 'z')
        {
            transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            zVel = moveDirection * moveSpeed;
        }
        rb.velocity = new Vector3(xVel, 0, zVel);
    }

    private void checkDespawn()
    {
        float currX = transform.position.x;
        float currZ = transform.position.z;
        if (currX < (-despawnDistance * GameSettings.blockSize) || currZ < (-despawnDistance * GameSettings.blockSize)
            || currX > (GameSettings.gridX + despawnDistance) * GameSettings.blockSize
            || currZ > (GameSettings.gridZ + despawnDistance) * GameSettings.blockSize)
        {
            enemySpawner.despawnEnemy(this.gameObject, "Bird");
        }
    }
}
