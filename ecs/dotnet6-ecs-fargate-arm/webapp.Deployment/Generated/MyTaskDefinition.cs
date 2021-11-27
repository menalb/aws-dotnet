using Amazon.CDK.AWS.ECS;
using Constructs;

namespace webapp.Deployment
{
    public class MyFargateTaskDefinition : FargateTaskDefinition
    {
        public MyFargateTaskDefinition(Construct scope, string id, IFargateTaskDefinitionProps? props = null) : base(scope, id, props)
        {
            
        }
    }
}
