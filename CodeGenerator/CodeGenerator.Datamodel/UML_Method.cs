﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Datamodel
{
    public class UML_Method : UML_Attribute
    {
        // List to store method parameters, may be empty
        public List<UML_Parameter> parameters { get; set; }

    }
}