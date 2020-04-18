using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieControls : MonoBehaviour
{
    public Camera mainCamera;

    public List<ZombieController> selectedZombies = new List<ZombieController>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var clickPosition3d = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var clickPosition = new Vector2(clickPosition3d.x, clickPosition3d.y);
            Collider2D[] objectsAtPosition = Physics2D.OverlapCircleAll(clickPosition, 0.01f);
            ZombieController selectedZombie = null;
            foreach (var possibleZombie in objectsAtPosition)
            {
                var zombie = possibleZombie.GetComponentInParent<ZombieController>();
                if (zombie != null) selectedZombie = zombie;
            }

            if (selectedZombie != null)
            {
                // Selected a zombie, change our selection and move on.
                ClearSelectedZombies();
                selectedZombie.IsSelected = true;
                selectedZombies.Add(selectedZombie);
            }
            else
            {
                // Clicked somewhere in the space. Tell selected zombies to go there.
                foreach(var zombie in selectedZombies)
                {
                    zombie.destination = clickPosition;
                }
            }
        }
    }

    public void ClearSelectedZombies()
    {
        foreach(var zombie in selectedZombies)
        {
            zombie.IsSelected = false;
        }
        selectedZombies.Clear();
    }
}
