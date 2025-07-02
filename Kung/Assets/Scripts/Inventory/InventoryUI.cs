using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


class InventoryUI : MonoBehaviour
{
    private GraphicRaycaster _gr;
    private PointerEventData _ped;
    private List<RaycastResult> _rrList;

    private InventorySlotUI _beginDragSlot; // 현재 드래그를 시작한 슬롯
    private Transform _beginDragIconTransform; // 해당 슬롯의 아이콘 트랜스폼

    private Vector3 _beginDragIconPoint;   // 드래그 시작 시 슬롯의 위치
    private Vector3 _beginDragCursorPoint; // 드래그 시작 시 커서의 위치
    private int _beginDragSlotSiblingIndex;

    private void Update()
    {
        _ped.position = Input.mousePosition;

        OnPointerDown();
        OnPointerDrag();
        OnPointerUp();
    }

    private T RaycastAndGetFirstComponent<T>() where T : Component
    {
        _rrList.Clear();

        _gr.Raycast(_ped, _rrList);

        if (_rrList.Count == 0)
            return null;

        return _rrList[0].gameObject.GetComponent<T>();
    }

    private void OnPointerDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _beginDragSlot = RaycastAndGetFirstComponent<InventorySlotUI>();

            if (_beginDragSlot != null && !_beginDragSlot.isEmpty)
            {
                _beginDragIconTransform = _beginDragSlot.transform;
                _beginDragIconPoint = _beginDragIconTransform.position;
                _beginDragCursorPoint = Input.mousePosition;

                _beginDragSlotSiblingIndex = _beginDragSlot.transform.GetSiblingIndex();
                _beginDragSlot.transform.SetAsLastSibling();
            }
            else
            {
                _beginDragSlot = null;
            }
        }
    }

    private void OnPointerDrag()
    {
        if (_beginDragSlot == null) return;

        if (Input.GetMouseButton(0))
        {
            _beginDragIconTransform.position =
                _beginDragIconPoint + (Input.mousePosition - _beginDragCursorPoint);
        }
    }

    private void OnPointerUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (_beginDragSlot != null)
            {
                _beginDragIconTransform.position = _beginDragIconPoint;

                _beginDragSlot.transform.SetSiblingIndex(_beginDragSlotSiblingIndex);

                _beginDragSlot = null;
                _beginDragIconTransform = null;
            }
        }
    }
}
