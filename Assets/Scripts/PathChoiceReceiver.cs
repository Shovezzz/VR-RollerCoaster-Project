using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; 

public class PathChoiceReceiver : MonoBehaviour
{
    public MultiFollowerSwitcher switcher;

    // � ���� ���� � ���������� �������, �� ����� ���� �������� ������
    public bool isDetourPathTarget = false;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        if (switcher == null)
        {
            Debug.LogError("Switcher �� �������� � ���������� ������� " + gameObject.name);
        }
    }

    void OnEnable()
    {
        interactable.selectEntered.AddListener(OnSelected);
    }

    void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnSelected);
    }

    private void OnSelected(SelectEnterEventArgs args)
    {
        Debug.Log("КЛИК ЗАРЕГИСТРИРОВАН на объекте: " + gameObject.name +
              ". Выбран обходной путь: " + isDetourPathTarget);
        switcher.MakeChoice(isDetourPathTarget);
    }
}