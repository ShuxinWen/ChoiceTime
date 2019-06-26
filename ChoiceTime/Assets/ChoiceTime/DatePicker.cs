using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 当选择日期的委托
/// </summary>
public delegate void OnDateUpdate();

public enum DateType
{
    _year, _month, _day,
    _hour, _minute, _second
}
/// <summary>
/// 日期选择器
/// </summary>
public class DatePicker : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// 日期类型 （年月日时分秒）
    /// </summary>
    public DateType _dateType;
    /// <summary>
    /// 子节点数量（奇数）
    /// </summary>
    public int _itemNum = 5;
    [HideInInspector]
    /// <summary>
    /// 更新选择的目标值
    /// </summary>
    public float _updateLength;
    /// <summary>
    /// 子节点预制体
    /// </summary>
    public GameObject _itemObj;
    /// <summary>
    /// 子节点容器对象
    /// </summary>
    public Transform _itemParent;
    /// <summary>
    /// 我属于的日期选择组
    /// </summary>
    [HideInInspector]
    public DatePickerGroup myGroup;
    /// <summary>
    /// 当选择日期的委托事件
    /// </summary>
    public event OnDateUpdate _onDateUpdate;
    void Awake()
    {
        _itemObj.SetActive(false);
        _updateLength = _itemObj.GetComponent<RectTransform>().sizeDelta.y;
    }

    /// <summary>
    /// 初始化时间选择器
    /// </summary>
    public void Init()
    {
        for (int i = 0; i < _itemNum; i++)
        {
            //计算真实位置
            int real_i = -(_itemNum / 2) + i;
            SpawnItem(real_i);
        }
        RefreshDateList();
    }
    /// <summary>
    /// 生成子对象
    /// </summary>
    /// <param name="dataNum">对应日期</param>
    /// <param name="real_i">真实位置</param>
    /// <returns></returns>
    GameObject SpawnItem(float real_i)
    {
        GameObject _item = Instantiate(_itemObj);
        _item.SetActive(true);
        _item.transform.SetParent(_itemParent);
        _item.transform.localScale = new Vector3(1, 1, 1);
        _item.transform.localEulerAngles = Vector3.zero;
        if (real_i != 0)
        {
            _item.GetComponent<Text>().color = new Color(1, 1, 1, 1 - 0.2f - (Mathf.Abs(real_i) / (_itemNum / 2 + 1)));
        }
        return _item;
    }

    Vector3 oldDragPos;
    /// <summary>
    /// 当开始拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        oldDragPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("拖拽");
        UpdateSelectDate(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _itemParent.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 更新选择的时间
    /// </summary>
    /// <param name="eventData"></param>
    void UpdateSelectDate(PointerEventData eventData)
    {
        //判断拖拽的长度是否大于目标值
        if (Mathf.Abs(eventData.position.y - oldDragPos.y) >= _updateLength)
        {
            int num;
            //判断加减
            if (eventData.position.y > oldDragPos.y)
            {
                //+
                num = 1;
            }
            else
            {
                //-
                num = -1;
            }
            DateTime _data = new DateTime();
            switch (_dateType)
            {
                case DateType._year:
                    _data = myGroup._selectDate.AddYears(num);
                    break;
                case DateType._month:
                    _data = myGroup._selectDate.AddMonths(num);
                    break;
                case DateType._day:
                    _data = myGroup._selectDate.AddDays(num);
                    break;
                case DateType._hour:
                    _data = myGroup._selectDate.AddHours(num);
                    break;
                case DateType._minute:
                    _data = myGroup._selectDate.AddMinutes(num);
                    break;
                case DateType._second:
                    _data = myGroup._selectDate.AddSeconds(num);
                    break;
            }
            //判断是否属于时间段
            if (IsInDate(_data, myGroup._minDate, myGroup._maxDate))
            {
                myGroup._selectDate = _data;
                oldDragPos = eventData.position;
                _onDateUpdate();
            }
            //   _itemParent.localPosition = Vector3.zero;
        }
        else
        {
            //_itemParent.localPosition = new Vector3(0, (eventData.position.y - oldDragPos.y), 0);
        }
    }

    /// <summary>
    /// 刷新时间列表
    /// </summary>
    public void RefreshDateList()
    {
        DateTime _data = new DateTime();
        for (int i = 0; i < _itemNum; i++)
        {
            //计算真实位置
            int real_i = -(_itemNum / 2) + i;
            switch (_dateType)
            {
                case DateType._year:
                    _data = myGroup._selectDate.AddYears(real_i);
                    break;
                case DateType._month:
                    _data = myGroup._selectDate.AddMonths(real_i);
                    break;
                case DateType._day:
                    _data = myGroup._selectDate.AddDays(real_i);
                    break;
                case DateType._hour:
                    _data = myGroup._selectDate.AddHours(real_i);
                    break;
                case DateType._minute:
                    _data = myGroup._selectDate.AddMinutes(real_i);
                    break;
                case DateType._second:
                    _data = myGroup._selectDate.AddSeconds(real_i);
                    break;
            }
            string str = "";
            if (IsInDate(_data, myGroup._minDate, myGroup._maxDate))
            {
                switch (_dateType)
                {
                    case DateType._year:
                        str = _data.Year.ToString();
                        break;
                    case DateType._month:
                        str = _data.Month.ToString();
                        break;
                    case DateType._day:
                        str = _data.Day.ToString();
                        break;
                    case DateType._hour:
                        str = _data.Hour.ToString();
                        break;
                    case DateType._minute:
                        str = _data.Minute.ToString();
                        break;
                    case DateType._second:
                        str = _data.Second.ToString();
                        break;
                }
            }
            _itemParent.GetChild(i).GetComponent<Text>().text = str;
        }

    }
    /// <summary> 
    /// 判断某个日期是否在某段日期范围内，返回布尔值
    /// </summary> 
    /// <param name="dt">要判断的日期</param> 
    /// <param name="dt_min">开始日期</param> 
    /// <param name="dt_max">结束日期</param> 
    /// <returns></returns>  
    public bool IsInDate(DateTime dt, DateTime dt_min, DateTime dt_max)
    {
        return dt.CompareTo(dt_min) >= 0 && dt.CompareTo(dt_max) <= 0;
    }
}

