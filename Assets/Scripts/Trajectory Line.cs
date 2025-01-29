
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [SerializeField] LineRenderer _trajectoryLine;
    [SerializeField] Transform _spawnpoint;

    private void Start()
    {
        _trajectoryLine.positionCount = 2;
    }

    private void Update()
    {
        _trajectoryLine.SetPosition(0, new Vector3(_spawnpoint.position.x, _spawnpoint.position.y, 0f));
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _trajectoryLine.SetPosition(1, 2 * (mousePos - (Vector2)_spawnpoint.position).normalized + (Vector2)_spawnpoint.position);
    }

    public void StartDisplay()
    {
        _trajectoryLine.enabled = true;
    }

    public void StopDisplay()
    {
        _trajectoryLine.enabled = false;
    }
}
