using UnityEngine;
using UnityEngine.UI;

public class KpiItemBehaviour : MonoBehaviour
{
    [SerializeField] 
    private Text kpiName, progress;

    [SerializeField] 
    private Image fill;

    [SerializeField] 
    private Color low, mid, full;

    public void SetValues(string kpiName, float progress, string color)
    {
        this.kpiName.text = kpiName;
        this.progress.text = $"{progress} %";
        fill.fillAmount = progress/100;

        switch (color)
        {
            case "Red":
                fill.color = low;
                break;
            
            case "Yellow":
                fill.color = mid;
                break;
            
            case "Green":
                fill.color = full;
                break;
        }
        
        /*if (progress >= 0.6f)
        {
            fill.color = full;
        }
        else if (progress >= 0.3f)
        {
            fill.color = mid;
        }
        else
        {
            fill.color = low;
        }*/
    }
}
