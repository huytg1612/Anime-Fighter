using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PointerDectection : MonoBehaviour
{
    private GraphicRaycaster gr;
    private PointerEventData pointerEventData = new PointerEventData(null);

    // Start is called before the first frame update
    void Start()
    {
        gr = GetComponent<GraphicRaycaster>();
    }

    // Update is called once per frame
    void Update()
    {
        pointerEventData.position = Camera.main.WorldToScreenPoint(transform.position);
        List<RaycastResult> results = new List<RaycastResult>();

        gr.Raycast(pointerEventData, results);

        Debug.Log(123);

        if(results.Count > 0)
        {
            Debug.Log(456);

            print(results[0].gameObject.name);
        }
    }
}
