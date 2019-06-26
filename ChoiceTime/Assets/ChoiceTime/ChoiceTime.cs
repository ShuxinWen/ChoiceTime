using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceTime : MonoBehaviour
{
    [Tooltip("箭头图片")]
    public Sprite[] Arrows;
    //显示时间的文字
    private Text ShowText;
    //显示箭头的图片
    private Image ArrowsImage;
    //选择时间界面
    private GameObject ChoiceTimeObj;
    //按钮
    private Button ChoiceBtn;
    //是否选择时间
    private bool isShowChoiceTime;

    // Use this for initialization
    void Start()
    {
        ShowText = GameObject.Find("Text").GetComponent<Text>();
        ArrowsImage = GameObject.Find("ShowArrows").GetComponent<Image>();
        ChoiceTimeObj = GameObject.Find("TimeData");
        ChoiceBtn = GameObject.Find("Button").GetComponent<Button>();
        ChoiceBtn.onClick.AddListener(StartChoiceTime);
        //开始默认选择系统时间
        ShowText.text = DateTime.Now.ToString("yyyy年MM月dd日 HH : mm : ss");
        ChoiceTimeObj.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (ChoiceTimeObj.activeSelf)
        {
            isShowChoiceTime = true;
        }
        else
        {
            isShowChoiceTime = false;
        }
    }


    public void StartChoiceTime()
    {
        //作战时间的number为1,开始时间的number为2   
        if (!isShowChoiceTime)
        {
            //显示选择时间界面
            ChoiceTimeObj.SetActive(true);
            //箭头向下
            ArrowsImage.sprite = Arrows[1];


        }
        else
        {
            //关闭选择时间界面
            ChoiceTimeObj.SetActive(false);
            //是否显示时间选择界面为false
            isShowChoiceTime = false;
            //箭头向上
            ArrowsImage.sprite = Arrows[0];
            //判断选没选择日期，当只点开选择框没有选择时，默认的日期会变为001年。所以要判断下
            if (DatePickerGroup._selectTime.ToString("yyyy年MM月dd日 HH : mm : ss").Substring(0, 3) == "000")
            {
                ShowText.text = DateTime.Now.ToString("yyyy年MM月dd日 HH : mm : ss");
            }
            else
            {
                ShowText.text = DatePickerGroup._selectTime.ToString("yyyy年MM月dd日 HH : mm : ss");
            }
        }
    }
}
