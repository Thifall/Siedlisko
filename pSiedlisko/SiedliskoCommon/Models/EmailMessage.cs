using SiedliskoCommon.DataValidators;
using SiedliskoCommon.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiedliskoCommon.Models
{
    public class EmailMessage
    {
        #region Fields and Properties
        [Required]
        public int Id { get; set; }
        [Required]
        public string ToAdress { get; set; }
        [Required]
        public string ToLogin { get; set; }
        public string MessageBody { get; set; }
        [Required]
        public EmailStatus status { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime SendTime { get; set; }
        [Required]
        [IsNotZeroOrLess]
        public int ReservationId { get; set; }
        #endregion
    }
}
