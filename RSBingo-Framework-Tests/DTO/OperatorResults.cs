using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSBingo_Framework_Tests.DTO;

public record class OperatorResults(Type? OperatorExceptionType, List<Type> WarningTypes);
