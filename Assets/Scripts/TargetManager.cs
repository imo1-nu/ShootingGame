using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GameObject[] targets;
    private GameObject currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnTargetRoutine());
    }

    IEnumerator SpawnTargetRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            if (currentTarget == null)
            {
                currentTarget = Instantiate(targets[0], new Vector3(0, 0, 5), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
