using Siedlisko.Models;
using Siedlisko.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siedlisko.ViewModels
{
    public class EditViewModel : IViewModel
    {
        public Reservation Reservation { get; set; }
        public bool OperationSucces { get; set; }
        public string OperationResultsDescription { get; set; }
    }
}
