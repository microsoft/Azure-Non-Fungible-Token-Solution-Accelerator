// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

@description('Provide a globally unique name of your VirtualMachine')
param virtualMachineName string

@description('Provide a name for Network Interface.')
param networkInterfaceName string = '${virtualMachineName}-nic'

@description('Provide a name for network security group.')
param networkSecurityGroupName string = '${virtualMachineName}-nsg'

@description('Provide a location for Virtual Machine.')
param location string = resourceGroup().location

@description('Provide Virtual Machine Size')
param vmSize string = 'Standard_B2ms'

@description('Admin User Name for Virtual Machine')
param adminUserName string = ''

@secure()
param adminPwd string = ''

param diskId string = newGuid()

param pipAddressId string

param vnetSubnetId string

resource networkSecurityGroupName_resource 'Microsoft.Network/networkSecurityGroups@2020-11-01' = {
  name: networkSecurityGroupName
  location: location
  tags: {
    displayName: 'Network Security Group'
  }
  properties: {
    securityRules: [
      {
        name: 'allow-http'
        properties: {
          description: 'Allow http'
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '80'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 301
          direction: 'Inbound'
          sourcePortRanges: []
          destinationPortRanges: []
          sourceAddressPrefixes: []
          destinationAddressPrefixes: []
        }
      }
      {
        name: 'allow-eth-ports'
        properties: {
          description: 'Allow standard ethereum ports'
          protocol: 'Tcp'
          sourcePortRange: '*'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 300
          direction: 'Inbound'
          sourcePortRanges: []
          destinationPortRanges: [
            '8545'
            '8546'
            '8547'
            '18545'
          ]
          sourceAddressPrefixes: []
          destinationAddressPrefixes: []
        }
      }
      {
        name: 'allow-monitoring-ports'
        properties: {
          description: 'Allow monitoring tools'
          protocol: 'Tcp'
          sourcePortRange: '*'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 302
          direction: 'Inbound'
          sourcePortRanges: []
          destinationPortRanges: [
            '8999'
            '5601'
            '3000'
            '9090'
            '25000'
          ]
          sourceAddressPrefixes: []
          destinationAddressPrefixes: []
        }
      }
      {
        name: 'allow-privacy'
        properties: {
          description: 'Allow Graphql'
          protocol: 'Tcp'
          sourcePortRange: '*'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 303
          direction: 'Inbound'
          sourcePortRanges: []
          destinationPortRanges: [
            '9081'
            '9082'
            '9083'
          ]
          sourceAddressPrefixes: []
          destinationAddressPrefixes: []
        }
      }
      {
        name: 'allow-privacy-examples'
        properties: {
          description: 'Allow privacy example ports'
          protocol: 'Tcp'
          sourcePortRange: '*'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 304
          direction: 'Inbound'
          sourcePortRanges: []
          destinationPortRanges: [
            '20000'
            '20001'
            '20002'
            '20003'
            '20004'
            '20005'
            '20006'
          ]
          sourceAddressPrefixes: []
          destinationAddressPrefixes: []
        }
      }
      {
        name: 'allow-ssh'
        properties: {
          description: 'Allow SSH'
          protocol: 'Tcp'
          sourcePortRange: '*'
          destinationPortRange: '22'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
          access: 'Allow'
          priority: 320
          direction: 'Inbound'
          sourcePortRanges: []
          destinationPortRanges: []
          sourceAddressPrefixes: []
          destinationAddressPrefixes: []
        }
      }
      // {
      //   name: 'Port_22'
      //   properties: {
      //     protocol: 'Tcp'
      //     sourcePortRange: '*'
      //     destinationPortRange: '22'
      //     sourceAddressPrefix: '76.104.244.241'
      //     destinationAddressPrefix: '*'
      //     access: 'Allow'
      //     priority: 319
      //     direction: 'Inbound'
      //     sourcePortRanges: []
      //     destinationPortRanges: []
      //     sourceAddressPrefixes: []
      //     destinationAddressPrefixes: []
      //   }
      // }
    ]
  }
}

resource virtualMachineName_resource 'Microsoft.Compute/virtualMachines@2021-07-01' = {
  name: virtualMachineName
  location: location
  properties: {
    hardwareProfile: {
      vmSize: vmSize
    }
    storageProfile: {
      imageReference: {
        publisher: 'Canonical'
        offer: 'UbuntuServer'
        sku: '18.04-LTS'
        version: 'latest'
      }
      osDisk: {
        osType: 'Linux'
        name: '${virtualMachineName}_OsDisk_${diskId}'
        createOption: 'FromImage'
        caching: 'ReadWrite'
        managedDisk: {
          storageAccountType: 'Standard_LRS'
        }
        deleteOption: 'Detach'
        diskSizeGB: 30
      }
      dataDisks: [
        {
          lun: 0
          name: '${virtualMachineName}-datadisk1'
          createOption: 'Attach'
          caching: 'None'
          writeAcceleratorEnabled: false
          managedDisk: {
            storageAccountType: 'StandardSSD_LRS'
            id: virtualMachineName_datadisk1.id
          }
          deleteOption: 'Detach'
          diskSizeGB: 4096
          toBeDetached: false
        }
      ]
    }
    osProfile: {
      computerName: virtualMachineName
      adminUsername: adminUserName
      adminPassword: adminPwd
      linuxConfiguration: {
        disablePasswordAuthentication: false
        provisionVMAgent: true
        patchSettings: {
          patchMode: 'ImageDefault'
          assessmentMode: 'ImageDefault'
        }
      }
      secrets: []
      allowExtensionOperations: true
    }
    networkProfile: {
      networkInterfaces: [
        {
          id: networkInterfaceName_resource.id
        }
      ]
    }
  }
}

resource virtualMachineName_datadisk1 'Microsoft.Compute/disks@2018-06-01' = {
  name: '${virtualMachineName}-datadisk1'
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    creationData: {
      createOption: 'Empty'
    }
    diskSizeGB: 4096
  }
}

resource virtualMachineName_AzureNetworkWatcherExtension 'Microsoft.Compute/virtualMachines/extensions@2021-07-01' = {
  parent: virtualMachineName_resource
  name: 'AzureNetworkWatcherExtension'
  location: location
  properties: {
    autoUpgradeMinorVersion: true
    publisher: 'Microsoft.Azure.NetworkWatcher'
    type: 'NetworkWatcherAgentLinux'
    typeHandlerVersion: '1.4'
  }
}

resource virtualMachineName_newuserscript 'Microsoft.Compute/virtualMachines/extensions@2021-07-01' = {
  parent: virtualMachineName_resource
  name: 'newuserscript'
  location: location
  properties: {
    autoUpgradeMinorVersion: true
    publisher: 'Microsoft.Azure.Extensions'
    type: 'CustomScript'
    typeHandlerVersion: '2.0'
    settings: {
      fileUris: [
        'https://catalogartifact.azureedge.net/publicartifacts/consensys.quorum-dev-quickstart-18d23e17-73e6-480d-b804-9997205d2651-quorum-dev-quickstart/Artifacts/scripts/qdq-setup.sh'
      ]
    }
    protectedSettings: {
      commandToExecute: '/bin/bash qdq-setup.sh ${adminUserName}'
    }
  }
}

resource networkSecurityGroupName_allow_eth_ports 'Microsoft.Network/networkSecurityGroups/securityRules@2020-11-01' = {
  parent: networkSecurityGroupName_resource
  name: 'allow-eth-ports'
  properties: {
    description: 'Allow standard ethereum ports'
    protocol: 'Tcp'
    sourcePortRange: '*'
    sourceAddressPrefix: '*'
    destinationAddressPrefix: '*'
    access: 'Allow'
    priority: 301
    direction: 'Inbound'
    sourcePortRanges: []
    destinationPortRanges: [
      '8545'
      '8546'
      '8547'
      '18545'
    ]
    sourceAddressPrefixes: []
    destinationAddressPrefixes: []
  }
}

resource networkSecurityGroupName_allow_http 'Microsoft.Network/networkSecurityGroups/securityRules@2020-11-01' = {
  parent: networkSecurityGroupName_resource
  name: 'allow-http'
  properties: {
    description: 'Allow http'
    protocol: 'Tcp'
    sourcePortRange: '*'
    destinationPortRange: '80'
    sourceAddressPrefix: '*'
    destinationAddressPrefix: '*'
    access: 'Allow'
    priority: 300
    direction: 'Inbound'
    sourcePortRanges: []
    destinationPortRanges: []
    sourceAddressPrefixes: []
    destinationAddressPrefixes: []
  }
}

resource networkSecurityGroupName_allow_monitoring_ports 'Microsoft.Network/networkSecurityGroups/securityRules@2020-11-01' = {
  parent: networkSecurityGroupName_resource
  name: 'allow-monitoring-ports'
  properties: {
    description: 'Allow monitoring tools'
    protocol: 'Tcp'
    sourcePortRange: '*'
    sourceAddressPrefix: '*'
    destinationAddressPrefix: '*'
    access: 'Allow'
    priority: 302
    direction: 'Inbound'
    sourcePortRanges: []
    destinationPortRanges: [
      '8999'
      '5601'
      '3000'
      '9090'
      '25000'
    ]
    sourceAddressPrefixes: []
    destinationAddressPrefixes: []
  }
}

resource networkSecurityGroupName_allow_privacy 'Microsoft.Network/networkSecurityGroups/securityRules@2020-11-01' = {
  parent: networkSecurityGroupName_resource
  name: 'allow-privacy'
  properties: {
    description: 'Allow Graphql'
    protocol: 'Tcp'
    sourcePortRange: '*'
    sourceAddressPrefix: '*'
    destinationAddressPrefix: '*'
    access: 'Allow'
    priority: 303
    direction: 'Inbound'
    sourcePortRanges: []
    destinationPortRanges: [
      '9081'
      '9082'
      '9083'
    ]
    sourceAddressPrefixes: []
    destinationAddressPrefixes: []
  }
}

resource networkSecurityGroupName_allow_privacy_examples 'Microsoft.Network/networkSecurityGroups/securityRules@2020-11-01' = {
  parent: networkSecurityGroupName_resource
  name: 'allow-privacy-examples'
  properties: {
    description: 'Allow privacy example ports'
    protocol: 'Tcp'
    sourcePortRange: '*'
    sourceAddressPrefix: '*'
    destinationAddressPrefix: '*'
    access: 'Allow'
    priority: 304
    direction: 'Inbound'
    sourcePortRanges: []
    destinationPortRanges: [
      '20000'
      '20001'
      '20002'
      '20003'
      '20004'
      '20005'
      '20006'
    ]
    sourceAddressPrefixes: []
    destinationAddressPrefixes: []
  }
}

resource networkSecurityGroupName_allow_ssh 'Microsoft.Network/networkSecurityGroups/securityRules@2020-11-01' = {
  parent: networkSecurityGroupName_resource
  name: 'allow-ssh'
  properties: {
    description: 'Allow SSH'
    protocol: 'Tcp'
    sourcePortRange: '*'
    destinationPortRange: '22'
    sourceAddressPrefix: '*'
    destinationAddressPrefix: '*'
    access: 'Allow'
    priority: 320
    direction: 'Inbound'
    sourcePortRanges: []
    destinationPortRanges: []
    sourceAddressPrefixes: []
    destinationAddressPrefixes: []
  }
}

resource networkSecurityGroupName_Port_22 'Microsoft.Network/networkSecurityGroups/securityRules@2020-11-01' = {
  parent: networkSecurityGroupName_resource
  name: 'Port_22'
  properties: {
    protocol: 'Tcp'
    sourcePortRange: '*'
    destinationPortRange: '22'
    sourceAddressPrefix: '76.104.244.241'
    destinationAddressPrefix: '*'
    access: 'Allow'
    priority: 319
    direction: 'Inbound'
    sourcePortRanges: []
    destinationPortRanges: []
    sourceAddressPrefixes: []
    destinationAddressPrefixes: []
  }
}

resource networkInterfaceName_resource 'Microsoft.Network/networkInterfaces@2020-11-01' = {
  name: networkInterfaceName
  location: location
  properties: {
    ipConfigurations: [
      {
        name: 'ipconfig1'
        properties: {
          privateIPAddress: '10.1.0.4'
          privateIPAllocationMethod: 'Dynamic'
          publicIPAddress: {
            id: pipAddressId 
          }
          subnet: {
            id: vnetSubnetId 
          }
          primary: true
          privateIPAddressVersion: 'IPv4'
        }
      }
    ]
    dnsSettings: {
      dnsServers: []
    }
    enableAcceleratedNetworking: false
    enableIPForwarding: false
    networkSecurityGroup: {
      id: networkSecurityGroupName_resource.id
    }
  }
}
