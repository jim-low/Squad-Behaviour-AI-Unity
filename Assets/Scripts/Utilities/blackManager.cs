using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blackManager : MonoBehaviour
{
    private GameObject[] allBlacks;
    private Soldier[] allBlackSoldiers;
    private float currentHealth;

    public void ReduceHP()
    {
        allBlacks = GameObject.FindGameObjectsWithTag("Enemy");

        allBlackSoldiers = new Soldier[allBlacks.Length];
        for (int i = 0; i < allBlacks.Length; i++)
        {
            allBlackSoldiers[i] = allBlacks[i].GetComponent<Soldier>();
        }

        foreach (Soldier soldier in allBlackSoldiers)
        {
            currentHealth = (soldier.getHealth() - ((float)(soldier.getMaxHealth() * 0.1)));
            soldier.setHealth(currentHealth);
            soldier.setHealthBar(currentHealth);
        }

    }

    public void HealToFull()
    {
        allBlacks = GameObject.FindGameObjectsWithTag("Enemy");

        allBlackSoldiers = new Soldier[allBlacks.Length];
        for (int i = 0; i < allBlacks.Length; i++)
        {
            allBlackSoldiers[i] = allBlacks[i].GetComponent<Soldier>();
        }

        foreach (Soldier soldier in allBlackSoldiers)
        {
            soldier.setHealth(soldier.getMaxHealth());
            soldier.setHealthBar(soldier.getMaxHealth());
        }
    }
}
