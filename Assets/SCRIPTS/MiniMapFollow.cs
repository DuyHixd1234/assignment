using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    public Transform player; 
    public float followSpeed = 10f; 

    private Vector3 offset;

    void Start()
    {
    
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        if (player != null)
        {
          
            Vector3 targetPosition = player.position + offset;
            targetPosition.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }
}
