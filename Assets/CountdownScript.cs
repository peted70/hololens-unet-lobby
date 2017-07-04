using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownScript : MonoBehaviour, ICountdown
{
    public TextMesh count;
    public void SetCountdown(int countdown)
    {
        if (countdown > 0)
        {
            gameObject.SetActive(true);
            count.text = countdown.ToString() + " seconds";
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
