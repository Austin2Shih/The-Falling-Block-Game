using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spray : MonoBehaviour
{
    private float sprayLingerTime;
    private float sprayStartTime;
    EnemySpawner enemySpawner;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = EnemySpawner.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.fixedTime > sprayStartTime + sprayLingerTime)
        {
            despawnSpray();
        }
    }

    public void setSprayLinger(float sprayLingerTime)
    {
        this.sprayLingerTime = sprayLingerTime;
    }

    public void spray()
    {
        sprayStartTime = Time.fixedTime;
    }

    private void despawnSpray()
    {
        enemySpawner.despawnEnemy(gameObject, "Spray");
    }

}
