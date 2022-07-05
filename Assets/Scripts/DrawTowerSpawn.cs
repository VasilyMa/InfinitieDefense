using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTowerSpawn : MonoBehaviour
{
    
    [Header("������ �� ������ ������")]
    [Tooltip("������� ���������� ������� � �� ������")]
    [SerializeField] private float[] _radiuses;
    [Header("���������� �����")]
    [SerializeField] private int _count;//test
    [Header("������ Gizmos'�")]
    [SerializeField] private Vector3 _boxSize;

    private float _angle = 0;
    private int _divisionColor = 1;
    private Vector3 _position;

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < _radiuses.Length; i++)
        {
            _angle = 0 + (90 * i);
            _divisionColor = 1 + i;
            Color drawColor = new Color(0.7f / _divisionColor, 0.7f / _divisionColor, 1, 1F);
            Gizmos.color = drawColor;
            for (int j = 0; j < _count; j++)
            {
                var x = Mathf.Cos(_angle * Mathf.Deg2Rad) * _radiuses[i];
                var z = Mathf.Sin(_angle * Mathf.Deg2Rad) * _radiuses[i];

                _angle += 360 / _count;

                _position = new Vector3(transform.position.x + x, transform.position.y + 0.5f, transform.position.z + z);
                Gizmos.DrawCube(_position, _boxSize);
            }
        }
        _divisionColor = 0;
        _angle = 0;
    }
}