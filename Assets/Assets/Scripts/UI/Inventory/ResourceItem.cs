using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceItem: MonoBehaviour
{
    public Image ItemImage;
    public TextMeshProUGUI CountText;

    public void Init(Sprite itemImage, int count)
    {
        ItemImage.sprite = itemImage;
        CountText.text = count.ToString();
    }

    public void SetRedCountText()
    {
        CountText.color = Color.red;
    }
}