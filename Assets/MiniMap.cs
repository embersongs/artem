using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    //�������� ���������
    [SerializeField] private float scrollSpeed = 1f;
    //����������� �����������
    [SerializeField] private float minValue = 10f;
    //������������ �����������
    [SerializeField] private float maxValue = 60f;
    //������� �����������
    private float currentValue;



    // Update is called once per frame
    void Update()
    {
        // �������� �������� ��������� �������� ����
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        // �������� �������� ���������� �������� ����������� � ����������� �� ����������� ���������
        if (scrollDelta > 0)
        {
            currentValue += scrollSpeed;
        }
        else if (scrollDelta < 0)
        {
            currentValue -= scrollSpeed;
        }
        // ������������ �������� ���������� ����� ����������� � ������������ ����������
        //�� ���� ������� �������� ����� �������������� �������, ������� �� ������ � �� ����� ������� � �������������
        currentValue = Mathf.Clamp(currentValue, minValue, maxValue);
        gameObject.GetComponent<Camera>().orthographicSize = currentValue;
    }
}