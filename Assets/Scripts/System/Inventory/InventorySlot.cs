using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour,
    IPointerClickHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [Header("Tipo de slot")]
    [SerializeField] private ItemType allowedItemType;

    [Header("Icono")]
    [SerializeField] private Image icon;

    private ItemData currentItem;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector3 originalPosition;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void SetItem(ItemData item)
    {
        currentItem = item;

        if (item != null && item.icon != null)
        {
            icon.sprite = item.icon;
            icon.color = Color.white;
        }
        else
        {
            icon.sprite = null;
            icon.color = Color.clear;
        }
    }

    public ItemData GetItem()
    {
        return currentItem;
    }

    public ItemType GetAllowedItemType()
    {
        return allowedItemType;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem == null)
            return;

        Debug.Log("Objeto seleccionado: " + currentItem.itemName);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem == null)
            return;

        // Por ahora solo permitimos arrastrar objetos normales.
        if (!CanDrag())
            return;

        originalParent = transform.parent;
        originalPosition = transform.position;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.7f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentItem == null || !CanDrag())
            return;

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentItem == null || !CanDrag())
            return;

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        transform.SetParent(originalParent);
        transform.position = originalPosition;
    }

    private bool CanDrag()
    {
        return currentItem.itemType != ItemType.Quest;
    }
}