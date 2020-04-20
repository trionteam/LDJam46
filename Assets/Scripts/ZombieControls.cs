using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieControls : MonoBehaviour
{
    public Camera mainCamera;
    public MainMenuController mainMenu;

    public HashSet<ZombieController> selectedZombies = new HashSet<ZombieController>();
    private bool selectedZombiesUpdating = false;

    public Transform dragDropMask;
    private bool isDragging = false;
    private Vector2 dragStartPosition;

    private Vector2 MousePosition2d
    {
        get
        {
            var mousePosition3d = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            return new Vector2(mousePosition3d.x, mousePosition3d.y);
        }
    }

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        Debug.Assert(mainCamera != null);

        if (mainMenu == null)
        {
            mainMenu = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<MainMenuController>();
        }

        if (dragDropMask == null)
        {
            dragDropMask = transform.Find("Selector");
        }
        Debug.Assert(dragDropMask != null);
    }

    public void SelectedZombie(ZombieController zombie, bool selected)
    {
        if (selectedZombiesUpdating) return;
        if (selected)
        {
            selectedZombies.Add(zombie);
        }
        else
        {
            selectedZombies.Remove(zombie);
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetMouseButtonDown(0) && !Input.GetMouseButtonUp(0))
        {
            isDragging = true;
            dragStartPosition = MousePosition2d;
        }
        else if (isDragging && Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            ClearSelectedZombies();

            Collider2D[] objectsAtPosition = Physics2D.OverlapAreaAll(dragStartPosition, MousePosition2d);
            foreach (var possibleZombie in objectsAtPosition)
            {
                var zombie = possibleZombie.GetComponentInParent<ZombieController>();
                if (zombie != null && zombie.isActiveAndEnabled) zombie.IsSelected = true;
            }
        }
        else if (!isDragging && Input.GetMouseButtonUp(1))
        {
            // Clicked somewhere in the space. Tell selected zombies to go there.
            foreach (var zombie in selectedZombies)
            {
                zombie.AssignDestination(MousePosition2d);
            }
			if (selectedZombies.Count > 0)
			{
				// TODO - sound wildcards
				SoundMgr.Instance?.Play($"lungs_{Random.Range(0, 2)}");
			}
        }

        dragDropMask.gameObject.SetActive(isDragging);
        if (isDragging)
        {
            var currentPosition = MousePosition2d;
            var center = new Vector3(0.5f * (currentPosition.x + dragStartPosition.x),
                                     0.5f * (currentPosition.y + dragStartPosition.y),
                                     9.0f);
            var size = new Vector3(Mathf.Abs(currentPosition.x - dragStartPosition.x),
                                   Mathf.Abs(currentPosition.y - dragStartPosition.y),
                                   1.0f);
            dragDropMask.position = center;
            dragDropMask.localScale = size;
        }
    }

    public void ClearSelectedZombies()
    {
        selectedZombiesUpdating = true;
        foreach (var zombie in selectedZombies)
        {
            zombie.IsSelected = false;
        }
        selectedZombies.Clear();
        selectedZombiesUpdating = false;
    }
}
