﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class CustomException : System.Exception
    {
        string message;

        public CustomException(string message)
        {
            this.message = message;
        }

        [DataMember]
        public string Message { get => message; set => message = value; }
    }
}
