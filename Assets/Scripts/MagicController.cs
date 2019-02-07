using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour {

    public Transform weaponHold;
    public MagicCaster defaultCaster;
    MagicCaster currentCaster;

    // Use this for initialization
    void Start()
    {
        if (defaultCaster != null)
            EquipMagicCaster(defaultCaster);
    }

    public void EquipMagicCaster(MagicCaster newCaster)
    {
        if (currentCaster != null)
            Destroy(currentCaster.gameObject);

        currentCaster = Instantiate(newCaster, weaponHold.position, weaponHold.rotation) as MagicCaster;
        currentCaster.transform.parent = weaponHold;
    }

    public void Cast(string attackValue)
    {
        if(currentCaster != null)
            currentCaster.Cast(attackValue);
    }
}
