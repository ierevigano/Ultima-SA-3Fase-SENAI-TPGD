using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;

    public Color normalColor = new Color(1f, 1f, 1f, 1f);
    public Color highlightedColor = new Color(1f, 1f, 1f, 1f);
    public Color selectedColor = new Color(1f, 1f, 1f, 1f);

    public bool IsToggledOn
    {
        get
        {
            return isToggledOn;
        }
    }

    private Image buttonImage;
    private bool isToggledOn = false;
    private bool isHovered = false;

    public event System.Action<bool> OnToggleChanged;

    /// <summary>
    /// Initializes references and sets up the button click listener.
    /// </summary>
    private void Awake()
    {
        if (button == null) button = GetComponent<Button>();
        if (buttonImage == null) buttonImage = GetComponent<Image>();

        button.onClick.AddListener(Toggle);
        UpdateVisual();
    }

    /// <summary>
    /// Called when the pointer enters the button area. Updates visual state.
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        UpdateVisual();
    }

    /// <summary>
    /// Called when the pointer exits the button area. Updates visual state.
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        UpdateVisual();
    }

    /// <summary>
    /// Toggles the button state and updates visuals. Triggers toggle event.
    /// </summary>
    private void Toggle()
    {
        isToggledOn = !isToggledOn;
        UpdateVisual();
        OnToggleChanged?.Invoke(isToggledOn);
    }

    /// <summary>
    /// Updates the button's color based on current toggle and hover states.
    /// </summary>
    private void UpdateVisual()
    {
        if (isToggledOn)
        {
            buttonImage.color = selectedColor;
        }
        else if (isHovered)
        {
            buttonImage.color = highlightedColor;
        }
        else
        {
            buttonImage.color = normalColor;
        }
    }
}