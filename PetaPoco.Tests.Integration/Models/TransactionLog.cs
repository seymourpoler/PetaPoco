﻿// <copyright company="PetaPoco - CollaboratingPlatypus">
//      Apache License, Version 2.0 https://github.com/CollaboratingPlatypus/PetaPoco/blob/master/LICENSE.txt
// </copyright>
// <author>PetaPoco - CollaboratingPlatypus</author>
// <date>2015/12/13</date>

using System;
using PetaPoco.Attributes;

namespace PetaPoco.Tests.Integration.Models
{
    [TableName("TransactionLogs")]
    public class TransactionLog
    {
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}