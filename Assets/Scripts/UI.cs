using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Button button;
    public Transform navCar;

    private bool activated = true;
    
    public void OnClick()
    {
        activated = !activated;
        if (false == activated)
        {
            button.GetComponent<Image>().color = new Color(0, 0, 0, 1.0f);
        }
        else
        {
            button.GetComponent<Image>().color = new Color(0, 0, 0, 0.4f);
        }
        navCar.gameObject.SetActive(activated);
    }
}
