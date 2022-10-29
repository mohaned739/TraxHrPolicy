using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TraxHrPolicy.DTOs;
using TraxHrPolicy.Models;
using TraxHrPolicy.Services;

namespace TraxHrPolicy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PolicyController : ControllerBase
    {
        private readonly traxhrContext _context;
        public PolicyController(traxhrContext conetext)
        {
             _context = conetext;
        }
        /// <summary>
        /// Used to generate a new Policy version from its parent policy 
        /// and the change type will be minor
        /// </summary>
        [Route("[action]")]
        [HttpPost]
        public IActionResult MinorUpdate()
        {
            try
            {
                var newPolicy = PolicyServices.AddNewVersion(_context, PolicyServices.VersionType.Minor);
                return Ok(newPolicy);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Used to generate a new Policy version from its parent policy 
        /// and the change type will be major
        /// </summary>
        [Route("[action]")]
        [HttpPost]
        public IActionResult MajorUpdate()
        {
            try
            {
                var newPolicy = PolicyServices.AddNewVersion(_context, PolicyServices.VersionType.Major);
                return Ok(newPolicy);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Used to generate a new Policy version, 
        /// the values of the new version are received from the user
        /// </summary>
        [Route("[action]")]
        [HttpPost]
        public IActionResult NewVersion([FromBody] PolicyDto policy)
        {
            try
            {
                var policies = _context.Policies.ToList();
                var latestPolicy = policies
                    .OrderBy(p => p.Id)
                    .LastOrDefault<Policy>(p => p.Published == true);

                if (latestPolicy == null)
                {
                    throw new NullReferenceException("There is Currently No Published Policies");
                }
                //Prevent the creation of new draft policy with older version
                if (float.Parse(policy.Version) <= float.Parse(latestPolicy.Version))
                {
                    return BadRequest("Cannot use older policy versions to create new policy");
                }

                var lastId = policies.Max(p => p.Id);
                var config = new MapperConfiguration(cfg => cfg.CreateMap<PolicyDto, Policy>());
                var mapper = new Mapper(config);
                var newPolicyVersion = mapper.Map<Policy>(policy);

                if (newPolicyVersion != null)
                {
                    newPolicyVersion.Parent = latestPolicy?.Id;
                    newPolicyVersion.Id = lastId + 1;
                    newPolicyVersion.Id1 = Guid.NewGuid().ToString();
                    if (float.Parse(policy.Version) % 1 == 0)
                    {
                        newPolicyVersion.ChangeType = "major";

                    }
                    else
                    {
                        newPolicyVersion.ChangeType = "minor";
                    }
                    newPolicyVersion.ParentType = latestPolicy.ContentType;
                    newPolicyVersion.TotalVersions = latestPolicy.TotalVersions + 1;
                    newPolicyVersion.Published = false;
                    newPolicyVersion.Created_at = DateTime.Now;
                }

                _context.Policies.Add(newPolicyVersion);
                _context.SaveChanges();
                var jsonPolicy = JsonSerializer.Serialize(newPolicyVersion);
                return Ok(jsonPolicy);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        /// <summary>
        /// Used to publish the draft version by updating 
        /// the published field for both the parent and draft policeis
        /// </summary>
        [Route("[action]")]
        [HttpPut]
        public IActionResult Promote()
        {
            try
            {
                var latestPolicy = _context.Policies
                    .OrderBy(p => p.Id)
                    .LastOrDefault<Policy>(p => p.Published == true);

                if (latestPolicy==null)
                {
                    throw new NullReferenceException("There is Currently No Published Policies");
                }

                var draftPolicy = _context.Policies
                    .OrderBy(p => p.Id)
                    .LastOrDefault<Policy>(p => p.Published == false);
                //Always the depricted versions will be below the currenly published version
                if (draftPolicy == null || draftPolicy.Id < latestPolicy.Id)
                {
                    throw new NullReferenceException("There is Currently No Draft Policies To Publish");
                }

                latestPolicy.Published = false;
                draftPolicy.Published = true;
                latestPolicy.Updated_at = DateTime.Now;
                draftPolicy.Updated_at = DateTime.Now;
                _context.SaveChanges();
                return Ok("Draft version has been published successfully");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
