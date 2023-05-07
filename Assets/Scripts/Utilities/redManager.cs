using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class redManager : MonoBehaviour
{
    private GameObject[] allReds;
    private Soldier[] allRedSoldiers;

    //public void ReduceHP()
    //{
    //    allReds = GameObject.FindGameObjectsWithTag("Ally");

    //    allRedSoldiers = new Soldier[allReds.Length];
    //    for (int i = 0; i < allReds.Length ; i++)
    //    {
    //        allRedSoldiers[i] = allReds[i].GetComponent<Soldier>();
    //    }

    //    foreach (Soldier soldier in allRedSoldiers) 
    //    {
    //        soldier.health -= (red.MAX_HEALTH * 0.1);
    //        soldier.soldierHealthBar.SetHealth(health);
    //    }

    //}

    //public void HealToFull()
    //{

    //}
}
