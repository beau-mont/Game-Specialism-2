using UnityEngine;

public class LerpToPosition : MonoBehaviour
{
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private float lerpSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * lerpSpeed);
    }
}
