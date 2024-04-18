using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.EmployeeSpecs
{
    public class EmployeeWithDepartmentSecifications : BaseSpecifications<Employee>
    {
        public EmployeeWithDepartmentSecifications():base()
        {
            Includes.Add(E=>E.Department);
        }
    }
}
