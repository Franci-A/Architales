using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeightManager : MonoBehaviour
{
    Vector2 horizontalBalance = Vector2.zero; // x = left; y = right
    Vector2 verticalBalance = Vector2.zero; // x = up; y = bottom

    [SerializeField] List<TextMeshProUGUI> DebugInfo = new List<TextMeshProUGUI>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < DebugInfo.Count; i++)
        {
            DebugInfo[i].transform.rotation = Quaternion.LookRotation(DebugInfo[i].transform.position - Camera.main.transform.position);

            switch (i) 
            {
                case 0:
                    DebugInfo[i].text = horizontalBalance.x.ToString();
                    break;

                case 1:
                    DebugInfo[i].text = horizontalBalance.y.ToString();
                    break; 

                case 2:
                    DebugInfo[i].text = verticalBalance.x.ToString();
                    break; 

                default:
                    DebugInfo[i].text = verticalBalance.y.ToString();
                    break;
            }
        }

    }
}
