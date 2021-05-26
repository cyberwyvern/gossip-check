using System.Collections.Generic;
using System.Threading.Tasks;

namespace GossipCheck.BLL.Interface
{
    public interface IStanceDetectorFascade
    {
        public Task<IEnumerable<KeyValuePair<string, Stance>>> GetSourceStances(string textOrigin);
    }
}
