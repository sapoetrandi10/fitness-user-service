using fitness_db.Data;
using fitness_db.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fitness_db.Repositories
{
    public class NutritionRepository : INutritionRepository
    {
        private FitnessContext _fitnessCtx;
        public NutritionRepository(FitnessContext context)
        {
            _fitnessCtx = context;
        }
    }
}
