using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesInSight1 = new List<GameObject>();
    [SerializeField] private HashSet<GameObject> enemiesInSight2 = new HashSet<GameObject>();

    


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInSight1.Add(other.gameObject);
            enemiesInSight2.Add(other.gameObject);
          
            foreach (var item in enemiesInSight2)
            {
                print(item);
            }
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
