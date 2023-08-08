using System.Linq;
using UserService.Models.Dto;
using UserService.Models.Entities;

namespace UserService.Helpers
{
    public static class SearchHelper
    {
        public static IQueryable<User> BuildSearchQuery(IQueryable<User> query,
            UserSearchOptions userSearchOptions)
        {
            if (userSearchOptions == null)
            {
                return query;
            }

            var newQuery = query;

            if (!string.IsNullOrEmpty(userSearchOptions.Username))
            {
                userSearchOptions.Username = userSearchOptions.Username.Trim();

                newQuery = newQuery.Where(x =>
                    x.Username.Contains(userSearchOptions.Username));
            }

            if (!string.IsNullOrEmpty(userSearchOptions.FirstName))
            {
                userSearchOptions.FirstName = userSearchOptions.FirstName.Trim();

                newQuery = newQuery.Where(x =>
                    x.FirstName.Contains(userSearchOptions.FirstName));
            }

            if (!string.IsNullOrEmpty(userSearchOptions.LastName))
            {
                userSearchOptions.LastName = userSearchOptions.LastName.Trim();

                newQuery = newQuery.Where(x =>
                    x.LastName.Contains(userSearchOptions.LastName));
            }

            if (!string.IsNullOrEmpty(userSearchOptions.Email))
            {
                userSearchOptions.Email = userSearchOptions.Email.Trim();

                newQuery = newQuery.Where(x =>
                    x.Email.Contains(userSearchOptions.Email));
            }

            if (userSearchOptions.Role != null)
            {
                newQuery = newQuery.Where(x => x.Role == userSearchOptions.Role.Value);
            }

            if (userSearchOptions.IsActive != null)
            {
                newQuery = newQuery.Where(x => x.IsActive == userSearchOptions.IsActive.Value);
            }

            return newQuery;
        }
    }
}
