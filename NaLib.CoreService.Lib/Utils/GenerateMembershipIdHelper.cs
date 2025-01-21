using System;

namespace NaLib.CoreService.Lib.Utils
{
    public class GenerateMembershipIdHelper
    {
        public static string GenerateMembershipId()
        {

            var uniqueId = Guid.NewGuid().ToString("N"); 
            return $"NaLib-{uniqueId.Substring(0, 8)}"; 
        }
    }
}
