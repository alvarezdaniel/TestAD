using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace TestAD
{
    // https://stackoverflow.com/questions/825237/how-can-you-find-a-user-in-active-directory-from-c
    public class ADHelper
    {
        public static bool CheckUserInAD(string domain, string username)
        {
            PrincipalContext domainContext = new PrincipalContext(ContextType.Domain, domain);
            UserPrincipal user = new UserPrincipal(domainContext);
            user.Name = username;
            PrincipalSearcher pS = new PrincipalSearcher();
            pS.QueryFilter = user;
            PrincipalSearchResult<Principal> results = pS.FindAll();
            if (results != null && results.Count() > 0)
                return true;
            return false;
        }

        public static bool CheckUserInAD2(string domain, string username)
        {
            using (var oDe = new DirectoryEntry("LDAP://" + domain, username, "", AuthenticationTypes.Secure))
            {
                return oDe != null;
            }
        }

        public static UserInfo GetUserData(string domain, string username)
        {
            using (var oDe = new DirectoryEntry("LDAP://" + domain, username, "", AuthenticationTypes.Secure))
            {
                var props = oDe.Properties;

                return new UserInfo
                {
                    FirstName = props["givenName"].Value.ToString(),
                    LastName = props["sn"].Value.ToString()
                };
            }
        }

        // https://www.codeproject.com/Articles/6778/How-to-get-User-Data-from-the-Active-Directory
        // https://www.manageengine.com/products/ad-manager/help/csv-import-management/active-directory-ldap-attributes.html
        public static UserInfo GetUserData2(string domain, string username)
        {
            using (var dir = new DirectoryEntry($"LDAP://{domain}"))
            {
                var search = new DirectorySearcher(dir);
                search.Filter = $"(&(objectClass=user)(sAMAccountName={username}))";
                var searchResult = search.FindOne();

                return new UserInfo
                {
                    FirstName = searchResult.Properties["givenName"][0].ToString(),
                    LastName = searchResult.Properties["sn"][0].ToString(),
                    Email = searchResult.Properties["mail"][0].ToString(),
                };
            }
        }
    }
}
