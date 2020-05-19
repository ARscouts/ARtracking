using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    public FloatVariable fillAmount;
    public GameObject loadingBar;
    public Image loadingBarFill;

    private void Update()
    {
        loadingBarFill.fillAmount = fillAmount.Value;
    }

    //public void SetActiveLoadingBar(bool value)
    //{
    //    loadingBar.SetActive(value);
    //}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void resetLoadingBar()
    {
        fillAmount.Value = 0.0f;
    }
}
