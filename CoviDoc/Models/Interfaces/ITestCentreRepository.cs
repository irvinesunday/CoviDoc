﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoviDoc.Models.Interfaces
{
    public interface ITestCentreRepository
    {
        List<TestCentre> GetTestCentres();
    }
}
