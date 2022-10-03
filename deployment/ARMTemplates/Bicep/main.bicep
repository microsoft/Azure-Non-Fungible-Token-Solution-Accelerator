// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

targetScope = 'subscription'

@description('Admin User Name for Virtual Machine')
param adminUserName string = ''

@description('The password must have 3 of the following: 1 lower case character, 1 upper case character, 1 number, and 1 special character that is not "#", "`", "*", "\'", "\'", "-", "%",\' \' or ";".')
@secure()
param adminPwd string = ''

@description('location for Resource deployment')
param paramLocation string = ''

@description('adding prefix to every resource names')
var resourceprefix = take(uniqueString(deployment().name, subscription().id),5)

@description('Provide a globally unique name of your VirtualMachine')
var virtualMachineName = 'vmnftsa${resourceprefix}'

resource rgQuorum 'Microsoft.Resources/resourceGroups@2020-10-01' = {
  name: 'blockchain-${resourceprefix}'
  location: paramLocation
}

module AzPublicIpDeploy 'BlockchainService/azurepublicadress.bicep' = {
  name: 'pip-${resourceprefix}'
  scope: rgQuorum
  params: {
    location: paramLocation
    virtualMachineName: virtualMachineName
  }
}

module AzVirtualNetworkDeploy 'BlockchainService/azurevirtualnetwork.bicep' = {
  name: 'vnet-${resourceprefix}'
  scope: rgQuorum
  params: {
    location: paramLocation
    virtualMachineName: virtualMachineName
  }
}

module AzVirtualMachineDeploy 'BlockchainService/azurevirtualmachine.bicep' = {
  name : 'vm-${resourceprefix}'
  scope: rgQuorum
  params:{
    location: paramLocation
    adminUserName: adminUserName
    adminPwd:adminPwd
    virtualMachineName: virtualMachineName
    pipAddressId: AzPublicIpDeploy.outputs.createdPipAddressId
    vnetSubnetId:AzVirtualNetworkDeploy.outputs.createdVnetSubnetId
  }
  dependsOn:[
    AzPublicIpDeploy
    AzVirtualNetworkDeploy
  ]
}

module trackingID 'BlockchainService/tracetag.bicep' = {
  name: 'pid-${resourceprefix}'   
  scope: rgQuorum
  params:{ }
}

resource rgTokenService 'Microsoft.Resources/resourceGroups@2020-10-01' = {
  name: 'nftservice-${resourceprefix}'
  location: paramLocation
}

module AzContainerRegistryDeploy 'NFTService/containerregistry.bicep' = {
  name: 'acr-${resourceprefix}'
  scope: rgTokenService
  params:{
    location: paramLocation
  }
}

module AzKubernetesClusterDeploy 'NFTService/azurekubernetesservice.bicep' = {
  name: 'aks-${resourceprefix}'
  scope: rgTokenService
  params:{
    location: paramLocation
  }
}

module NFTUserIdentityDeploy 'NFTService/nftuseridentity.bicep' = {
  name: 'UserIdentity-${resourceprefix}'
  scope: rgTokenService
  params: {
    location: paramLocation
  }
}

module AzCosmosDBDeploy 'NFTService/cosmosdb.bicep' = {
  name: 'cosmos-${resourceprefix}'
  scope: rgTokenService
  params: {
    location: paramLocation
    primaryRegion: paramLocation
  }
}

module AzureKeyVaultDeploy 'NFTService/azurekeyvault.bicep' = {
  name : 'akv-${resourceprefix}'
  scope: rgTokenService
  params: { 
    location: paramLocation
  }
}

module nfttrackingID 'NFTService/tracetag.bicep' = {
  name: 'pid-${resourceprefix}'   
  scope: rgTokenService
  params:{ }
}

output pipDomainName string = AzPublicIpDeploy.outputs.createdFqdnUri
output blockchainRgName string = rgQuorum.name
output vmName string = virtualMachineName
output aksName string = AzKubernetesClusterDeploy.outputs.createdAksName
output acrName string = AzContainerRegistryDeploy.outputs.createdAcrName
output cosmosName string = AzCosmosDBDeploy.outputs.createdCosmosDBName
output akvUrl string = AzureKeyVaultDeploy.outputs.createdAkvUri
output nftRgName string = rgTokenService.name
output nftUserIdentityClientId string = NFTUserIdentityDeploy.outputs.createdNftUserIdentityClientId
