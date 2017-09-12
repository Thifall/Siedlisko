using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siedlisko.ViewModels.Interfaces
{
    public interface IViewModel
    {
        bool OperationSucces { get; set; }
        string OperationResultsDescription { get; set; }
    }
}
