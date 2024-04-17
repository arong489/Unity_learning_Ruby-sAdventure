using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    private UnityEngine.UI.Image image;
    void Start()
    {
        this.image = GetComponent<Image>();
    }

    public void setFillAmount(float amount)
    {
        this.image.fillAmount = amount;
    }
}
