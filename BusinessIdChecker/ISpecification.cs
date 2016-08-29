using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessIdChecker
{
    public interface ISpecification<in TEntity>
    {

        IEnumerable<string> ReasonsForDissatisfaction { get; }

        bool IsSatisfiedBy(TEntity entity);

    }
}
