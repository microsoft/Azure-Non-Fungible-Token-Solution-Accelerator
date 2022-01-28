// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

param virtualMachineName string

@description('Provide a name for Virtual Network.')
param virtualNetworkName string = '${virtualMachineName}-vnet'

@description('Provide a location for Virtual Network')
param location string = resourceGroup().location

resource virtualNetworkName_resource 'Microsoft.Network/virtualNetworks@2020-11-01' = {
  name: virtualNetworkName
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [
        '10.1.0.0/20'
      ]
    }
    subnets: [
      {
        name: 'nftdevtestsubnet'
        properties: {
          addressPrefix: '10.1.0.0/20'
          delegations: []
          privateEndpointNetworkPolicies: 'Enabled'
          privateLinkServiceNetworkPolicies: 'Enabled'
        }
      }
    ]
    virtualNetworkPeerings: []
    enableDdosProtection: false
  }
}

resource virtualNetworkName_subnet 'Microsoft.Network/virtualNetworks/subnets@2020-11-01' = {
  parent: virtualNetworkName_resource
  name: 'nftdevtestsubnet'
  properties: {
    addressPrefix: '10.1.0.0/20'
    delegations: []
    privateEndpointNetworkPolicies: 'Enabled'
    privateLinkServiceNetworkPolicies: 'Enabled'
  }
}

output createdVnetSubnetId string = virtualNetworkName_subnet.id