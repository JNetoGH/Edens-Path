using System;
using UnityEngine;


public class DrinhaFlowerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Bouquet"))
            Invoke(nameof(FinishLevel), 0.3f);
    }

    private void FinishLevel()
    {
        FindObjectOfType<LevelOneProgressValidator>().hasDrinhaGotTheFlowers = true;
    }
}
