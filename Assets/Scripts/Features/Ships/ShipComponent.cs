using System.Collections.Generic;

namespace Client
{
    /// <summary>
    /// ����� �������, ������ ��������� �� ��
    /// </summary>
    struct ShipComponent
    {
        public int Number;
        public List<int> EnemyUnitsEntitys;
        public ShipArrivalMonoBehavior ShipArrivalMonoBehavior;
    }
}