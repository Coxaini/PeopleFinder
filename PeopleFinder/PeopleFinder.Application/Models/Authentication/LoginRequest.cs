using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace PeopleFinder.Application.Models.Authentication;

public record LoginRequest(
string EmailOrUsername,
string Password);
