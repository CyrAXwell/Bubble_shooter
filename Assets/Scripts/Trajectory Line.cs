
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [SerializeField] LineRenderer trajectoryLine;
    [SerializeField] Transform spawnpoint;

    private void Start()
    {
        trajectoryLine.positionCount = 2;
        
    }

    private void Update()
    {
        trajectoryLine.SetPosition(0, new Vector3(spawnpoint.position.x, spawnpoint.position.y, 0f));
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        trajectoryLine.SetPosition(1, 2 * (mousePos - (Vector2)spawnpoint.position).normalized + (Vector2)spawnpoint.position);
    }

    public void StartDisplay()
    {
        trajectoryLine.enabled = true;
    }

    public void StopDisplay()
    {
        trajectoryLine.enabled = false;
    }
}
