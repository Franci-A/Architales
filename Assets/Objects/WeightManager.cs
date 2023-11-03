using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeightManager : MonoBehaviour
{
    Vector2 horizontalBalance = Vector2.zero; // x = left; y = right
    Vector2 verticalBalance = Vector2.zero; // x = bottom; y = top

    [SerializeField] float maxBalance;

    [Header("Debug")]
    [SerializeField] List<TextMeshProUGUI> DebugInfo = new List<TextMeshProUGUI>();


    private static WeightManager instance;
    public static WeightManager Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }

    void Update()
    {
        UpdateDebug();
    }

    private void UpdateDebug()
    {
        for (int i = 0; i < DebugInfo.Count; i++)
        {
            DebugInfo[i].transform.rotation = Quaternion.LookRotation(DebugInfo[i].transform.position - Camera.main.transform.position);

            switch (i)
            {
                case 0:
                    DebugInfo[i].text = (-horizontalBalance.x).ToString();
                    break;

                case 1:
                    DebugInfo[i].text = horizontalBalance.y.ToString();
                    break;

                case 2:
                    DebugInfo[i].text = (-verticalBalance.x).ToString();
                    break;

                default:
                    DebugInfo[i].text = verticalBalance.y.ToString();
                    break;
            }
        }

        if (Mathf.Abs(horizontalBalance.x) - horizontalBalance.y > maxBalance)
        {
            DebugInfo[0].color = Color.red;
            DebugInfo[1].color = Color.red;
        }

        if (Mathf.Abs(verticalBalance.x) - verticalBalance.y > maxBalance)
        {
            DebugInfo[2].color = Color.red;
            DebugInfo[3].color = Color.red;
        }
    }

    public void UpdateWeight(Vector3 blockPosition)
    {
        if (blockPosition.x < 0) horizontalBalance.x += blockPosition.x;
        else horizontalBalance.y += blockPosition.x;

        if (blockPosition.z < 0) verticalBalance.x += blockPosition.z;
        else verticalBalance.y += blockPosition.z;
    }
}
