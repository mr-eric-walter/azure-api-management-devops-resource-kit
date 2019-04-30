﻿using System.Collections.Generic;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Create
{
    public class BackendTemplateCreator
    {
        private TemplateCreator templateCreator;

        public BackendTemplateCreator(TemplateCreator templateCreator)
        {
            this.templateCreator = templateCreator;
        }

        public Template CreateBackendTemplate(CreatorConfig creatorConfig)
        {
            // create empty template
            Template backendTemplate = this.templateCreator.CreateEmptyTemplate();

            // add parameters
            backendTemplate.parameters = new Dictionary<string, TemplateParameterProperties>
            {
                { "ApimServiceName", new TemplateParameterProperties(){ type = "string" } }
            };

            List<TemplateResource> resources = new List<TemplateResource>();
            foreach (BackendTemplateProperties backendTemplatePropeties in creatorConfig.backends)
            {
                // create backend resource with properties
                BackendTemplateResource backendTemplateResource = new BackendTemplateResource()
                {
                    name = $"[concat(parameters('ApimServiceName'), '/{backendTemplatePropeties.title}')]",
                    type = ResourceTypeConstants.Backend,
                    apiVersion = "2018-06-01-preview",
                    properties = backendTemplatePropeties,
                    dependsOn = new string[] { }
                };
                resources.Add(backendTemplateResource);
            }

            backendTemplate.resources = resources.ToArray();
            return backendTemplate;
        }
    }
}