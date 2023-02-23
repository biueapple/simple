using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UIController : MonoBehaviour
{
    //GraphicRaycaster
    GraphicRaycaster m_gr;
    PointerEventData m_ped;
    List<RaycastResult> results;

    public Canvas canvas;

    public List<GameObject> uiOpens = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        m_ped = new PointerEventData(null);
        m_gr = canvas.GetComponent<GraphicRaycaster>();
        results = new List<RaycastResult>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void OpenUI(GameObject uiObj)
    {
        uiObj.SetActive(true);
        uiOpens.Add(uiObj);
    }
    public void CloseUI(GameObject uiObj)
    {
        uiObj.gameObject.SetActive(false);
        if(uiOpens.Contains(uiObj))
        {
            uiOpens.Remove(uiObj);
        }
    }
    public void CloseUI()
    {
        if(uiOpens.Count > 0)
        {
            uiOpens[0].SetActive(false);
            uiOpens.RemoveAt(0);
        }
    }


    public T GetGraphicRay<T>()
    {
        results.Clear();

        m_ped.position = Input.mousePosition;

        m_gr.Raycast(m_ped, results);

        if (results.Count > 0)
        {
            return results[0].gameObject.GetComponent<T>();
        }
        return default(T);
    }
    public T GetGraphicRay<T>(bool b)
    {
        results.Clear();

        m_ped.position = Input.mousePosition;

        m_gr.Raycast(m_ped, results);

        if (results.Count > 0)
        {
            for(int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject.transform.GetComponent<T>() != null)
                {
                    return results[i].gameObject.GetComponent<T>();
                }
            }
        }
        return default(T);
    }

    public void ListSwap<T>(List<T> list, int index1, int index2)
    {
        if (index2 == -1)
            return;

        if (index1 < 0 || index2 >= list.Count)
        {
            return;
        }

        T item = list[index1];
        list[index1] = list[index2];
        list[index2] = item;

    }
    public int FindIndex<T>(List<T> list, T obj)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Equals(obj))
            {
                return i;
            }
        }
        return -1;
    }

    public Canvas GetCanvas()
    {
        return canvas;
    }
}
