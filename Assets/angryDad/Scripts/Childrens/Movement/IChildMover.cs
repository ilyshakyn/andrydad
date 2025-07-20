using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChildMover 
{
    void MoveTo(Vector3 position);
    bool HasReachedDestination();
}
