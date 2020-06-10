using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValuesChange : MonoBehaviour
{
    public Slider ClueCountSlider;
    public Slider MaxSlider;
    public Slider MinSlider;
    public Text ClueCountInfo;
    public Text MaxInfo;
    public Text MinInfo;

    public IntVariable ClueCount;
    public FloatVariable MaxTrackingDistance;
    public FloatVariable MinTrackingDistance;

    void Start()
    {
        ClueCountSlider.value = ClueCount.Value;
        ClueCountUpdate();
        MaxSlider.value = MaxTrackingDistance.Value;
        MaxUpdate();
        MinSlider.value = MinTrackingDistance.Value;
        MinUpdate();

    }

    public void ClueCountUpdate()
    {
        ClueCountInfo.text = ClueCountSlider.value.ToString();
        ClueCount.Value = (int)ClueCountSlider.value;

    }
    public void MaxUpdate()
    {
        MaxInfo.text = MaxSlider.value.ToString();
        MaxTrackingDistance.Value = MaxSlider.value;

    }
    public void MinUpdate()
    {
        MinInfo.text = MinSlider.value.ToString();
        MinTrackingDistance.Value = MinSlider.value;

    }

    void Update()
    {
        
    }
}
