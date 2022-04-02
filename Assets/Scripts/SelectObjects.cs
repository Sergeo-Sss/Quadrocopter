using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

public class SelectObjects : MonoBehaviour {

	public static List<GameObject> unit; // массив всех юнитов, которых мы можем выделить
	public static List<GameObject> unitSelected; // массив выделенных юнитов

	public GUISkin skin;
	private Rect rect;
	private bool draw;
	private Vector2 startPos;
	private Vector2 endPos;

    public TMP_Dropdown groups;
    public GameObject destroy;
    public TMP_InputField vvodgroup;

    private bool checkinputgroup = false;

    List<string> m_DropOptions = new List<string> { "Option 1" };

    void Awake () 
	{
		unit = new List<GameObject>();
		unitSelected = new List<GameObject>();
	}

	// проверка, добавлен объект или нет
	bool CheckUnit (GameObject unit) 
	{
		bool result = false;
		foreach(GameObject u in unitSelected)
		{
			if(u == unit) result = true;
		}
		return result;
	}

	void Select()
	{
		if(unitSelected.Count > 0)
		{
			for(int j = 0; j < unitSelected.Count; j++)
			{
                // делаем что-либо с выделенными объектами
                unitSelected[j].GetComponent<Outline>().enabled = true;
			}
		}

    }

	void Deselect()
	{
		if(unitSelected.Count > 0)
		{
			for(int j = 0; j < unitSelected.Count; j++)
			{
                // отменяем то, что делали с объектоми
                unitSelected[j].GetComponent<Outline>().enabled = false;
			}
		}
      
    }

    public void PaintGroup()
    {
        for (int i=1;i<groups.options.Count;i++)
        {
            GameObject go = GameObject.Find(groups.options[i].text);

            if (i==groups.value)
            {
                for (int k = 0; k < go.transform.childCount;k++)
                {
                    go.transform.GetChild(k).GetComponent<Outline>().enabled = true;
                }
                continue;
            }
            else
            {
                for (int k = 0; k < go.transform.childCount; k++)
                {
                    go.transform.GetChild(k).GetComponent<Outline>().enabled = false;
                }
            }
        }
    }

    private void RepaintDestroy()
    {
        GameObject go = GameObject.Find(groups.options[groups.value].text);

        for (int k = 0; k < go.transform.childCount; k++)
        {
            go.transform.GetChild(k).GetComponent<Outline>().enabled = false;
        }
    }

    private void Update()
    {
        if (unitSelected.Count>0 && Input.GetMouseButtonDown(1))
        {
            GameObject go = vvodgroup.transform.gameObject;
            go.transform.position = Input.mousePosition;
            go.SetActive(true);
        }
        if (groups.value==0)
        {
            destroy.SetActive(false);
        }
        else
        {
            
            destroy.SetActive(true);
        }

        if (vvodgroup.transform.gameObject.active && Input.GetMouseButtonDown(0) && !checkinputgroup)
        {
            Deselect();
            startPos = Input.mousePosition;
            draw = true;
            vvodgroup.transform.gameObject.SetActive(false);
        }
    }

    public void CreateGroup(GameObject but)
    {
        TMP_InputField input = but.GetComponent<TMP_InputField>();

        if (input.text!="")
        {
            m_DropOptions.Clear();
            m_DropOptions.Add(input.text);
            groups.AddOptions(m_DropOptions);


            //че делаем со списком
            GameObject newgroup = new GameObject(input.text);

            for (int i = 0; i < unitSelected.Count; i++)
            {
                unitSelected[i].transform.parent = newgroup.transform;
            }
        }
        

        Deselect();
        startPos = Input.mousePosition;
        draw = true;

        GameObject go = vvodgroup.transform.gameObject;
        go.SetActive(false);
    }

    public void Test(bool check)
    {
        checkinputgroup = check;
    }

    public void ClearGroup()
    {
        string txt = groups.options[groups.value].text;
        GameObject parent = GameObject.Find(txt);

        RepaintDestroy();
        while (parent.transform.childCount>0)
        {
            parent.transform.GetChild(0).SetParent(null);
        }

        Destroy(GameObject.Find(txt));
        groups.options.RemoveAt(groups.value);
        groups.value = 0;
    }

    void OnGUI ()
	{
		GUI.skin = skin;
		GUI.depth = 99;

		if(Input.GetMouseButtonDown(0))
		{
            if (!vvodgroup.transform.gameObject.active)
            {
                Deselect();
                startPos = Input.mousePosition;
                draw = true;
            }
		}

		if (Input.GetMouseButtonUp(0)) 
		{
			draw = false;
			Select();
		}
		
		if(draw)
		{
			unitSelected.Clear();
			endPos = Input.mousePosition;
			if(startPos == endPos) return;

			rect = new Rect(Mathf.Min(endPos.x, startPos.x),
			                Screen.height - Mathf.Max(endPos.y, startPos.y),
			                Mathf.Max(endPos.x, startPos.x) - Mathf.Min(endPos.x, startPos.x),
			                Mathf.Max(endPos.y, startPos.y) - Mathf.Min(endPos.y, startPos.y)
			                );
			
			GUI.Box(rect, "");

			for(int j = 0; j < unit.Count; j++)
			{
				// трансформируем позицию объекта из мирового пространства, в пространство экрана
				Vector2 tmp = new Vector2(Camera.main.WorldToScreenPoint(unit[j].transform.position).x, Screen.height - Camera.main.WorldToScreenPoint(unit[j].transform.position).y);

				if(rect.Contains(tmp)) // проверка, находится-ли текущий объект в рамке
				{
					if(unitSelected.Count == 0)
					{
						unitSelected.Add(unit[j]);
					}
					else if(!CheckUnit(unit[j]))
					{
						unitSelected.Add(unit[j]);
					}
				}
			}
		}
	}
}
