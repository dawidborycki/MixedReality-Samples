#region Using

using HoloToolkit.Unity.InputModule;
using UnityEngine;

#endregion

public class SimpleInteraction : MonoBehaviour, IInputClickHandler, IFocusable
{
    #region Properties

    public Material AlternativeMaterial;

    #endregion

    #region Fields

    private bool isAlternativeMaterialApplied = false;
    private Vector3 originalScale;
    private Material originalMaterial;
    private Renderer renderer;

    #endregion

    #region Focus

    public void OnFocusEnter()
    {
        transform.localScale = (originalScale + Vector3.one * 0.05f);
    }

    public void OnFocusExit()
    {
        transform.localScale = originalScale;
    }

    #endregion

    #region Input clicked

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (AlternativeMaterial != null)
        {
            if (isAlternativeMaterialApplied)
            {
                renderer.material = originalMaterial;
            }
            else
            {
                renderer.material = AlternativeMaterial;
            }

            isAlternativeMaterialApplied = !isAlternativeMaterialApplied;
        }
    }

    #endregion

    #region Awake

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        originalScale = transform.localScale;
        originalMaterial = renderer.material;
    }

    #endregion
}