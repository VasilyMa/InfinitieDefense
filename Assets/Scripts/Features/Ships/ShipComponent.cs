using System.Collections.Generic;

namespace Client
{
    /// <summary>
    /// Номер корабля, массив сущностей на нём
    /// </summary>
    struct ShipComponent
    {
        public int Number;
        public List<int> EnemyUnitsEntitys;
        public ShipArrivalMonoBehavior ShipArrivalMonoBehavior;
    }
}