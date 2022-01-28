// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

@minLength(5)
@maxLength(50)
@description('Provide a globally unique name of your Azure kubernetes Cluster')
param aksName string = 'aksnftsa${uniqueString(resourceGroup().id)}'

@description('Provide a location for aks.')
param location string = resourceGroup().location

resource aks 'Microsoft.ContainerService/managedClusters@2021-05-01' = {
  name: aksName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    dnsPrefix: aksName
    enableRBAC: true
    kubernetesVersion: '1.20.9'
    agentPoolProfiles: [
      {
        name: 'tokensvc'
        count: 3
        vmSize: 'Standard_A4_v2'
        mode: 'System'
      }
    ]
  }
}

output createdAksName string = aksName
