// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

param virtualMachineName string

@description('Provide a name for public ip adddress.')
param publicIPAddresseName string = '${virtualMachineName}-pip'

@description('Provide a location for public Ip Address')
param location string = resourceGroup().location

resource publicIPAddresseName_resource 'Microsoft.Network/publicIPAddresses@2020-11-01' = {
  name: publicIPAddresseName
  location: location
  properties: {
    publicIPAddressVersion: 'IPv4'
    publicIPAllocationMethod: 'Dynamic'
    idleTimeoutInMinutes: 4
    dnsSettings: {
      domainNameLabel: virtualMachineName
    }
  }
}

output createdPipAddressId string = publicIPAddresseName_resource.id
output createdFqdnUri string = publicIPAddresseName_resource.properties.dnsSettings.fqdn

output Web_block_explorer_address string = 'http://${publicIPAddresseName_resource.properties.dnsSettings.fqdn}:25000/'
output Prometheus_address string = 'http://${publicIPAddresseName_resource.properties.dnsSettings.fqdn}:9090/'
output Grafana_address string = 'http://${publicIPAddresseName_resource.properties.dnsSettings.fqdn}:3000/'
output Kibana_address string = 'http://${publicIPAddresseName_resource.properties.dnsSettings.fqdn}:5601/'
output Cakeshop_address string = 'http://${publicIPAddresseName_resource.properties.dnsSettings.fqdn}:8999/'
output JSON_RPC_HTTP_Service_Endpoint string = 'http://${publicIPAddresseName_resource.properties.dnsSettings.fqdn}:8545/'
output JSON_RPC_WebSocket_service_endpoint string = 'http://${publicIPAddresseName_resource.properties.dnsSettings.fqdn}:8546/'
output GraphQL_HTTP_service_endpoint string = 'http://${publicIPAddresseName_resource.properties.dnsSettings.fqdn}:8547/'