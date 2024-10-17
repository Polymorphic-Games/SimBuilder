using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonRunModelScript : MonoBehaviour
{
    [SerializeField] Sprite playIcon;
    [SerializeField] Sprite pauseIcon;
    int toggle = 1;
    void Start()
    {
        //playIcon.GetComponent<Image>().preserveAspect = true;
        //pauseIcon.GetComponent<Image>().preserveAspect = true;
        gameObject.GetComponent<Image>().sprite = playIcon;
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        toggle *= -1;

        if (toggle == 1)
        {
            gameObject.GetComponent<Image>().sprite = playIcon;
        } else
        {
            gameObject.GetComponent<Image>().sprite = pauseIcon;
        }
    }

}
