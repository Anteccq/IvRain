using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IvRain.Models.Services;

namespace IvRain.Models.Parameter
{
    public record PasswordManagePageParameter(IBlockService Service, IEnumerable<Block> Blocks, string Password);
}
