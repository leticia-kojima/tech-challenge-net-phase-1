using FCG.Application._Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.Contracts.Catalogs.Commands;

public class DeleteCatalogCommandRequest : ICommand
{
    public Guid Key { get; set; }
}
