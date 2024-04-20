using System.Collections.Generic;

namespace SideProject.MapEdit
{
    [System.Serializable]
    public sealed class MapItem
    {
        public string name;
        public List<PointItem> pointList;
    }
}
