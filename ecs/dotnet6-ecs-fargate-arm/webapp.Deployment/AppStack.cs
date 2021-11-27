// Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.ECS;
using AWS.Deploy.Recipes.CDK.Common;

using webapp.Deployment.Configurations;
using static Amazon.CDK.AWS.ECS.CfnTaskDefinition;

namespace webapp.Deployment
{
    public class AppStack : Stack
    {
        private readonly Configuration _configuration;

        internal AppStack(Construct scope, IDeployToolStackProps<Configuration> props)
            : base(scope, props.StackName, props)
        {
            _configuration = props.RecipeProps.Settings;

            // Setup callback for generated construct to provide access to customize CDK properties before creating constructs.
            CDKRecipeCustomizer<Recipe>.CustomizeCDKProps += CustomizeCDKProps;

            // Create custom CDK constructs here that might need to be referenced in the CustomizeCDKProps. For example if
            // creating a DynamoDB table construct and then later using the CDK construct reference in CustomizeCDKProps to
            // pass the table name as an environment variable to the container image.

            // Create the recipe defined CDK construct with all of its sub constructs.
            var generatedRecipe = new Recipe(this, props.RecipeProps);

            // Create additional CDK constructs here. The recipe's constructs can be accessed as properties on
            // the generatedRecipe variable.
        }

        /// <summary>
        /// This method can be used to customize the properties for CDK constructs before creating the constructs.
        ///
        /// The pattern used in this method is to check to evnt.ResourceLogicalName to see if the CDK construct about to be created is one
        /// you want to customize. If so cast the evnt.Props object to the CDK properties object and make the appropriate settings.
        /// </summary>
        /// <param name="evnt"></param>
        private void CustomizeCDKProps(CustomizePropsEventArgs<Recipe> evnt)
        {
            if (evnt.Construct.AppTaskDefinition != null)
            {
                Console.WriteLine(evnt.Construct.AppTaskDefinition.Node.Id);
                try
                {

                    var cfnTaskDefinition = (evnt.Construct.AppTaskDefinition?.Node.DefaultChild as Amazon.CDK.AWS.ECS.CfnTaskDefinition);

                    if (cfnTaskDefinition != null)
                    {
                        var a = new RuntimePlatformProperty { CpuArchitecture = "ARM64", OperatingSystemFamily = "LINUX" };

                        cfnTaskDefinition.RuntimePlatform = a;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed!");
                    Console.WriteLine(e.Message);
                    throw e;
                }
            }


            // Example of how to customize the container image definition to include environment variables to the running applications.
            // 
            //if (string.Equals(evnt.ResourceLogicalName, nameof(evnt.Construct.AppContainerDefinition)))
            //{
            //    if(evnt.Props is ContainerDefinitionOptions props)
            //    {
            //        Console.WriteLine("Customizing AppContainerDefinition");
            //        if (props.Environment == null)
            //            props.Environment = new Dictionary<string, string>();

            //        props.Environment["EXAMPLE_ENV1"] = "EXAMPLE_VALUE1";
            //    }
            //}
        }
    }
}
