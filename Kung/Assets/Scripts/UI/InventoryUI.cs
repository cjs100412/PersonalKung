using System.Collections;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform enterTarget;
    public Transform quitTarget;

    public void OnInventoryEnterButton()
    {
        StartCoroutine(EnterInventoryUI());
    }
    public void OnInventoryQuitButton()
    {
        StartCoroutine(QuitInventoryUI());
    }

    IEnumerator EnterInventoryUI()
    {
        while (Vector3.Distance(transform.localPosition, enterTarget.localPosition) > 0.1f){
            transform.localPosition = Vector3.Lerp(transform.localPosition, enterTarget.localPosition, Time.deltaTime * 10);
            yield return null;
        }
        transform.localPosition = enterTarget.localPosition;
    }
    IEnumerator QuitInventoryUI()
    {
        while (Vector3.Distance(transform.localPosition, quitTarget.localPosition) > 0.1f){
            transform.localPosition = Vector3.Lerp(transform.localPosition, quitTarget.localPosition, Time.deltaTime * 10);
            yield return null;
        }
        transform.localPosition = quitTarget.localPosition;
    }
}
