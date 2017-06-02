using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCreator.Enumerators;

namespace TestCreator.Events
{
    public class ToolBarEventArgs : EventArgs
    {
        public ToolbarOption Option { get; set; } 
    }
}
