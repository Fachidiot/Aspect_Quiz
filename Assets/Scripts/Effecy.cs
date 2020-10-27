using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effecy : MonoBehaviour
{
    void Update()
    {
        gameObject.GetComponent<Text>().color -= new Color(0, 0, 0, 1f) * Time.deltaTime;
        if(gameObject.GetComponent<Text>().color.a <= 0)
        {
            gameObject.GetComponent<Text>().color = new Color(0, 0, 0, 1);
            gameObject.SetActive(false);
        }
    }
}
