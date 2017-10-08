using SiedliskoCommon.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiedliskoCommon.Models
{
    public class EmailMessage
    {
        #region Fields and Properties
        public int Id { get; set; }
        public string ToAdress { get; set; }
        public string ToLogin { get; set; }
        public string MessageBody { get; set; }
        public EmailStatus status { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime SendTime { get; set; }
        public int ReservationId { get; set; }
        #endregion
    }
}
