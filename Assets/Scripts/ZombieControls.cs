using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieControls : MonoBehaviour
{
    public Camera mainCamera;

    public HashSet<ZombieController> selectedZombies = new HashSet<ZombieController>();
    private bool selectedZombiesUpdating = false;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        Debug.Assert(mainCamera != null);
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

        if (Input.GetMouseButtonUp(0))
        {
            var clickPosition3d = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var clickPosition = new Vector2(clickPosition3d.x, clickPosition3d.y);
            Collider2D[] objectsAtPosition = Physics2D.OverlapCircleAll(clickPosition, 0.01f);
            ZombieController selectedZombie = null;
            foreach (var possibleZombie in objectsAtPosition)
            {
                var zombie = possibleZombie.GetComponentInParent<ZombieController>();
                if (zombie != null && zombie.isActiveAndEnabled) selectedZombie = zombie;
            }

            if (selectedZombie != null)
            {
                // Selected a zombie, change our selection and move on.
                ClearSelectedZombies();
                selectedZombie.IsSelected = true;                
            }
            else
            {
                // Clicked somewhere in the space. Tell selected zombies to go there.
                foreach(var zombie in selectedZombies)
                {
                    zombie.AssignDestination(clickPosition);
                }
            }
        }
    }

    public void ClearSelectedZombies()
    {
        selectedZombiesUpdating = true;
        foreach(var zombie in selectedZombies)
        {
            zombie.IsSelected = false;
        }
        selectedZombies.Clear();
        selectedZombiesUpdating = false;
    }
}
