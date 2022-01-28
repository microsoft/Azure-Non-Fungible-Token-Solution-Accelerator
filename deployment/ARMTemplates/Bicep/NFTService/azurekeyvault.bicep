// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

@minLength(5)
@maxLength(50)
@description('Provide a globally unique name of your Azure Keyvault')
param akvName string = 'akvnftsa${uniqueString(resourceGroup().id)}'

@description('Provide a location for Key Vault.')
param location string = resourceGroup().location

param param_tenantId string = subscription().tenantId

resource kv 'Microsoft.KeyVault/vaults@2021-06-01-preview' = {
  name: akvName
  location: location
 
  properties: {
    accessPolicies: []
    createMode: 'default'
    enableSoftDelete: true
    sku: {
      family: 'A'
      name: 'standard'
    }
    softDeleteRetentionInDays: 7
    tenantId: param_tenantId
    
  }
}

output createdAkvUri string = kv.properties.vaultUri
