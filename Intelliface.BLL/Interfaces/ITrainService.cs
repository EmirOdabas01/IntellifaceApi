using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.BLL.Interfaces
{
    public interface ITrainService
    {
            Task TrainModelAsync(int employeeId);
    }
}
