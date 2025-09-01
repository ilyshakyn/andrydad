using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneSensor : MonoBehaviour
{
    [SerializeField] private string _playerTag = "Player";

    public event Action PlayerEntered;
    public event Action PlayerExited;

    private int _insideCount = 0; // на случай нескольких коллайдеров игрока

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_playerTag))
        {
            _insideCount++;
            if (_insideCount == 1) PlayerEntered?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_playerTag))
        {
            _insideCount = Mathf.Max(0, _insideCount - 1);
            if (_insideCount == 0) PlayerExited?.Invoke();
        }
    }

    // ≈сли у теб€ 2D-физика Ч можно оставить и эти методы, они просто не вызовутс€ в 3D.
  

    private void OnDrawGizmosSelected()
    {
        // ѕросто подсветим границы зоны
        Gizmos.color = new Color(1, 0.3f, 0.2f, 0.25f);
        var col = GetComponent<Collider>();
        if (col != null) Gizmos.DrawCube(col.bounds.center, col.bounds.size);
    }
}
