using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGManager.Models
{
    public class NavigationItem
    {
        public string Title { get; set; } = null!;
        public string Icon { get; set; } = null!;
        public Type ViewModelType { get; set; } = null!;
    }
}
