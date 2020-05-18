﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static CoviDoc.Models.Enums;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace CoviDoc.Models
{
    /// <summary>
    /// Cannot update the IdNumber/Names/DateRegistered of patient
    /// </summary>
    public class Patient
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string IdNumber { get; set; }
        public DateTime DoB { get; set; }

        [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
        public Gender Gender { get; set; } = Gender.Uknown;

        public string Nationality { get; set; }
        public string MobileNumber { get; set; }
        public string County { get; set; }
        public string Constitiency { get; set; }
        public string Ward { get; set; }
        public bool IsAdult { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateRegistered { get; set; }

    }
}
