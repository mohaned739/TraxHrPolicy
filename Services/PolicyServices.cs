using System.Text.Json;
using TraxHrPolicy.Models;

namespace TraxHrPolicy.Services
{
    public class PolicyServices
    {
        public enum VersionType
        {
            Minor,
            Major
        }
        /// <summary>
        /// Used to aapply the minor and major updated on the already created version
        /// </summary>
        /// <param name="context"></param>
        /// <param name="option"></param>
        /// <returns>The value that has been added</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public static string AddNewVersion(traxhrContext context, VersionType option)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            var policies = context.Policies.ToList();
            var parentPolicy = policies
                .OrderBy(p => p.Id)
                .LastOrDefault<Policy>(p => p.Published == true);
            var Id = policies.Max(p => p.Id);

            if (parentPolicy == null)
            {
                throw new NullReferenceException("There is Currently No Published Policies");
            }
            var newPolicyVersion = parentPolicy;
            newPolicyVersion.Parent = parentPolicy?.Id;
            newPolicyVersion.Id = Id + 1;
            newPolicyVersion.Id1 = Guid.NewGuid().ToString();

            if (option == VersionType.Minor)
            {
                var newVersion = float.Parse(parentPolicy.Version);
                newVersion += 0.1f;
                newPolicyVersion.Version = newVersion.ToString();
                newPolicyVersion.ChangeType = "minor";
            }
            else if (option == VersionType.Major)
            {
                var newVersion = Math.Floor(float.Parse(parentPolicy.Version)) + 1;
                newPolicyVersion.Version = newVersion.ToString();
                newPolicyVersion.ChangeType = "major";
            }

            newPolicyVersion.ParentType = parentPolicy?.ContentType;
            newPolicyVersion.TotalVersions = parentPolicy.TotalVersions + 1;
            newPolicyVersion.Published = false;
            newPolicyVersion.Created_at = DateTime.Now;

            context.Policies.Add(newPolicyVersion);
            context.SaveChanges();
            return JsonSerializer.Serialize(newPolicyVersion);
        }
    }
}
