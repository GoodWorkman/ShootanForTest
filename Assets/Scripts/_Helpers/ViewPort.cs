using UnityEngine;

public class ViewPort : Singleton<ViewPort>
{
    private float _minX, _middleX, _maxX, _minY, _maxY;
    public float MaxX => _maxX;

    private void Start() {
        Camera mainCamera = Camera.main;

        if (mainCamera)
        {
            Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f));
            Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f));
            
            _middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).x;

            _minX = bottomLeft.x;
            _maxX = topRight.x;
            _minY = bottomLeft.y;
            _maxY = topRight.y;
        }
    }

    public Vector3 PlayerMoveablePosition(Vector3 playerPosition, 
        float paddingX, float paddingY) 
    {
        Vector3 position = Vector3.zero;
        position.x = Mathf.Clamp(playerPosition.x, _minX + paddingX, _maxX - paddingX);
        position.y = Mathf.Clamp(playerPosition.y, _minY + paddingY, _maxY - paddingY);
        return position;
    }

    public Vector3 RandomEnemySpawnPosition(float paddingX, float paddingY) 
    {
        return new Vector3(
            _maxX + paddingX, 
            Random.Range(_minY + paddingY, _maxY - paddingY)
        );
    }

    public Vector3 RandomRightHalfPosition(float paddingX, float paddingY) 
    {
        return new Vector3(
            Random.Range(_middleX, _maxX - paddingX),
            Random.Range(_minY + paddingY, _maxY - paddingY)
        );
    }

}
