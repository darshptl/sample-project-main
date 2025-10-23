using System.Collections.Generic;
using System.Linq;
using BusinessEntities;
using Common;

namespace Core.Services.Users
{
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class UpdateUserService : IUpdateUserService
    {
        public void Update(User user, string name, string email, UserTypes type, decimal? annualSalary, int? age, IEnumerable<string> tags)
        {
            //update email only if its not null. Otherwise it ends up in an exception
            if (email != null && email != "")
            {
                user.SetEmail(email);
            }

            //update name only if its not null. Otherwise it ends up in an exception
            if (name != null && name != "")
            {
                user.SetName(name);
            }

            user.SetType(type);

            decimal? monthly = annualSalary.HasValue ? (decimal?)(annualSalary.Value / 12m) : null;
            user.SetMonthlySalary(monthly);

            if (age.HasValue)
            {
                user.SetAge(age.Value);
            }

            // Only update tags when caller provides a non-null, non-empty collection.
            // null means "leave unchanged". If you want to allow explicit clearing, accept [] as clear.
            if (tags != null && tags.Any()) 
            {
                user.SetTags(tags);
            }

        }
    }
}