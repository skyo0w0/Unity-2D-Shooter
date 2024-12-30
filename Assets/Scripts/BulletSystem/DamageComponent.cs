using System.Collections;
using System.Collections.Generic;
using DeathResponce;
using Player;
using UIComponents;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class DamageComponent : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<IDeathResponse>() != null)
        {
            collision.gameObject.GetComponent<IDeathResponse>().OnDeath();
            
            Destroy(this.gameObject);
        }
    }
}
