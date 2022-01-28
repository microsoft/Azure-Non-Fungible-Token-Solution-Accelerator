// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

@description('Provide Managed Idenity user name.')
param nftUserIdentityName string = 'NFTUserIdentity-${uniqueString(resourceGroup().id)}'

@description('Location for creating managed identity.')
param location string = resourceGroup().location

resource nftIdentity_resource 'Microsoft.ManagedIdentity/userAssignedIdentities@2018-11-30' = {
  name: nftUserIdentityName
  location: location
}

output createdNftUserIdentity string= nftIdentity_resource.id
output createdNftUserIdentityId string= nftIdentity_resource.properties.principalId
output createdNftUserIdentityClientId string= nftIdentity_resource.properties.clientId
