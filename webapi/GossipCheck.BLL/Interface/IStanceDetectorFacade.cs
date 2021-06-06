using GossipCheck.BLL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GossipCheck.BLL.Interface
{
    public interface IStanceDetectorFacade
    {
        public Task<IEnumerable<KeyValuePair<string, Stance>>> GetSourceStances(string textOrigin);
    }
}
