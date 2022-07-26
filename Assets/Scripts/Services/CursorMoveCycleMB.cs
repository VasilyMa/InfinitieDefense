using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorMoveCycleMB : MonoBehaviour
{
    [SerializeField] float distance;
    [SerializeField] float speedMove;
    [SerializeField] Vector3 direction;

    bool back = false;
    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, startPosition) < distance && !back)
        {
            transform.position += direction.normalized * speedMove * Time.deltaTime;
        }
        else
            back = true;

        if (back)
        {
            float tmp = Vector3.Distance(transform.position, startPosition);

            transform.position -= direction.normalized * speedMove * Time.deltaTime;

            if (Vector3.Distance(transform.position, startPosition) > tmp)
            {
                back = false;
            }
        }
        this.transform.LookAt(this.transform.position + Camera.main.transform.forward);
    }
}
