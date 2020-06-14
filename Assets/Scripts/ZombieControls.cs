using System.Collections.Generic;
using UnityEngine;

public class ZombieControls : MonoBehaviour
{
    [SerializeField]
    private Camera _mainCamera;

    [SerializeField]
    private MainMenuController _mainMenu;

    private HashSet<ZombieController> selectedZombies = new HashSet<ZombieController>();
    private bool selectedZombiesUpdating = false;

    private List<ZombieController> hoveredZombies = new List<ZombieController>();

    [SerializeField]
    private Transform _dragDropMask;

    private bool isDragging = false;
    private bool maybeStartedDragging = false;
    private Vector2 dragStartPosition;

    private Vector2 MousePosition2d
    {
        get
        {
            var mousePosition3d = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            return new Vector2(mousePosition3d.x, mousePosition3d.y);
        }
    }

    private void Awake()
    {
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        Debug.Assert(_mainCamera != null);

        if (_mainMenu == null)
        {
            _mainMenu = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<MainMenuController>();
        }
        Debug.Assert(_mainMenu != null);

        if (_dragDropMask == null)
        {
            _dragDropMask = transform.Find("Selector");
        }
        Debug.Assert(_dragDropMask != null);
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
        if (_mainMenu.IsActive)
        {
            isDragging = false;
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && !Input.GetMouseButtonUp(0))
            {
                maybeStartedDragging = true;
                dragStartPosition = MousePosition2d;
            }
            else if (maybeStartedDragging && Input.GetMouseButton(0))
            {
                if (dragStartPosition != MousePosition2d)
                {
                    isDragging = true;
                    maybeStartedDragging = false;
                }
            }
            else if (isDragging && Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                var zombies = GetSelectedZombies(dragStartPosition, MousePosition2d);
                // Interpret this as a click when the drag is small enough and no zombies were selected.
                if (zombies.Count > 0 || (MousePosition2d - dragStartPosition).magnitude > 0.2f)
                {
                    UpdateSelectedZombies(zombies);
                }
                else if (selectedZombies.Count > 0)
                {
                    SetDestinationForSelectedZombies();
                }
            }
            else if (!isDragging && Input.GetMouseButtonUp(0))
            {
                // First look for zombies under the mouse cursor.
                var zombies = GetSelectedZombies(MousePosition2d, MousePosition2d);
                if (zombies.Count > 0)
                {
                    UpdateSelectedZombies(zombies);
                }
                else
                {
                    SetDestinationForSelectedZombies();
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                SetDestinationForSelectedZombies();
            }
        }

        _dragDropMask.gameObject.SetActive(isDragging);
        if (isDragging)
        {
            var currentPosition = MousePosition2d;
            var center = new Vector3(0.5f * (currentPosition.x + dragStartPosition.x),
                                     0.5f * (currentPosition.y + dragStartPosition.y),
                                     9.0f);
            var size = new Vector3(Mathf.Abs(currentPosition.x - dragStartPosition.x),
                                   Mathf.Abs(currentPosition.y - dragStartPosition.y),
                                   1.0f);
            _dragDropMask.position = center;
            _dragDropMask.localScale = size;
        }

        foreach (var zombie in hoveredZombies)
        {
            zombie.IsHovered = false;
        }
        hoveredZombies.Clear();
        if (!isDragging && !_mainMenu.IsActive)
        {
            var zombies = GetSelectedZombies(MousePosition2d, MousePosition2d);
            foreach (var zombie in zombies)
            {
                if (!zombie.IsSelected)
                {
                    zombie.IsHovered = true;
                    hoveredZombies.Add(zombie);
                }
            }
        }
    }

    private void SetDestinationForSelectedZombies()
    {
        var mousePosition = MousePosition2d;
        // Try to find a target at the click position.
        var target = GetTargetAtPosition(mousePosition);

        // Clicked somewhere in the space. Tell selected zombies to go there.
        foreach (var zombie in selectedZombies)
        {
            if (target != null)
            {
                zombie.AssignTarget(target);
            }
            else
            {
                zombie.AssignDestination(mousePosition);
            }
        }
        if (selectedZombies.Count > 0)
        {
            // TODO - sound wildcards
            SoundMgr.Instance?.Play($"go_{Random.Range(0, 3)}");
        }
    }

    /// <summary>
    /// Finds a healthy person (that can be a target of a zombie) at a given position.
    /// </summary>
    /// <param name="position">The position, at which the healthy person is looked for.</param>
    /// <returns>The healthy person at the given position. Returns <c>null</c> if no
    /// healthy person is found at that position.</returns>
    private HealthyPersonController GetTargetAtPosition(Vector2 position)
    {
        Collider2D[] objectsAtPosition = Physics2D.OverlapCircleAll(position, 0.01f, 1 << Layers.Zombies);
        foreach (var possibleHealthyPerson in objectsAtPosition)
        {
            var healthyPerson = possibleHealthyPerson.GetComponentInParent<HealthyPersonController>();
            if (healthyPerson != null && healthyPerson.isActiveAndEnabled) return healthyPerson;
        }
        return null;
    }

    private List<ZombieController> GetSelectedZombies(Vector2 corner1, Vector2 corner2)
    {
        var list = new List<ZombieController>();
        Collider2D[] objectsAtPosition = Physics2D.OverlapAreaAll(corner1, corner2, 1 << Layers.Zombies);
        foreach (var possibleZombie in objectsAtPosition)
        {
            var zombie = possibleZombie.GetComponentInParent<ZombieController>();
            if (zombie != null && zombie.isActiveAndEnabled) list.Add(zombie);
        }
        return list;
    }

    private void UpdateSelectedZombies(IEnumerable<ZombieController> zombies)
    {
        ClearSelectedZombies();
        foreach (var zombie in zombies)
        {
            zombie.IsSelected = true;
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
