using UnityEngine;

public class EnergyBar : MonoBehaviour
{
    RectTransform rect;
    [SerializeField] Player player;
    [SerializeField] float width;
    [SerializeField] float height;
    float ratio;
    float prevRatio;
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }
    void Update()
    {
        ratio = player.Energy / player.MaxEnergy;
        if (ratio != prevRatio)
        {
            rect.sizeDelta = new Vector2(ratio * width, height);
            prevRatio = ratio;
        }
    }
}