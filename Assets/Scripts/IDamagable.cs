using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////
//Abstract definition for gameobjects that can take damage//
////////////////////////////////////////////////////////////

public interface IDamagable
{
    void TakeHit(float damage, RaycastHit hit, string attackValue);
    void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDir, string attackValue);
}