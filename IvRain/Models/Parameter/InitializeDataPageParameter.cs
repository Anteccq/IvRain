using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IvRain.Models.Services;
using IvRain.ViewModels;

namespace IvRain.Models.Parameter;

public record InitializeDataPageParameter(IBlockService Service, InitializeDataPageViewModel ViewModel);