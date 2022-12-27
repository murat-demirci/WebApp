using AutoMapper;
using Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Concrete
{
    public class ManagerBase
    {
        protected IUnitofWork UnitOfWork { get; }
        protected IMapper Mapper { get; }

        public ManagerBase(IUnitofWork unitofWork, IMapper mapper)
        {
            UnitOfWork = unitofWork;
            Mapper = mapper;
        }        
    }
}
