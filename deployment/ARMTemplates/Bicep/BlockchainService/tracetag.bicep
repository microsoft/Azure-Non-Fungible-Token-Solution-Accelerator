
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT licens

@description('create resource with NFT Trace ID')
resource traceTage_resource 'Microsoft.Resources/deployments@2021-04-01' = {
  name: 'pid-ea77555d-5efd-5b4b-ba1f-1d54583008ea'
  properties:{
    mode: 'Incremental'
    template:{
      '$schema': 'https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#'
      contentVersion: '1.0.0.0'
      resources: []
    }
  }
}
