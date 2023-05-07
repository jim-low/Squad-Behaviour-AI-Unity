using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class redManager : MonoBehaviour
{
    private GameObject[] allReds;
    private Soldier[] allRedSoldiers;
    private float currentHealth;

    public void ReduceHP()
    {
        allReds = GameObject.FindGameObjectsWithTag("Ally");

        allRedSoldiers = new Soldier[allReds.Length];
        for (int i = 0; i < allReds.Length; i++)
        {
            allRedSoldiers[i] = allReds[i].GetComponent<Soldier>();
        }

        foreach (Soldier soldier in allRedSoldiers)
        {
            /* currentHealth = (soldier.getHealth() - ((float)(soldier.getMaxHealth() * 0.1))); */
            soldier.Damage((float)(soldier.getMaxHealth() * 0.1));
            /* soldier.setHealthBar(currentHealth); */
        }

    }

    public void HealToFull()
    {
        allReds = GameObject.FindGameObjectsWithTag("Ally");

        allRedSoldiers = new Soldier[allReds.Length];
        for (int i = 0; i < allReds.Length; i++)
        {
            allRedSoldiers[i] = allReds[i].GetComponent<Soldier>();
        }

        foreach (Soldier soldier in allRedSoldiers)
        {
            soldier.setHealth(soldier.getMaxHealth());
            soldier.setHealthBar(soldier.getMaxHealth());
        }
    }
}
