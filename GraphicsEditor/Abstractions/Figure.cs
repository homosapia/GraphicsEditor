using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsEditor.Abstractions
{
    public abstract class Figure
    {
        public abstract List<object> CopyElements();

        public abstract void InsertElements(List<object> objects);
    }
}
