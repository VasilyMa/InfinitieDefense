using System.Collections.Generic;

namespace Client
{
    /// <summary>
    /// ����� �������, ������ ��������� �� ��
    /// </summary>
    struct ShipComponent
    {
        public int Number;
        public int Wave;
        public List<int> EnemyUnitsEntitys;
        public ShipArrivalMonoBehavior ShipArrivalMonoBehavior;
    }
}