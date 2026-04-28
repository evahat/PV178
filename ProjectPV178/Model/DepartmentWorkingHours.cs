using Microsoft.EntityFrameworkCore;

namespace ProjectPV178.Model
{
    [Owned]
    public class DepartmentWorkingHours
    {
        public int From { get; set; }
        public int To { get; set; }
    }
}
