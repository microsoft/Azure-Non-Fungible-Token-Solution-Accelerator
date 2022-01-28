# Copyright (c) Microsoft Corporation.
# Licensed under the MIT license.

param(
    [Parameter(Mandatory= $True,
                HelpMessage='Enter the Azure subscription ID to deploy your resources')]
    [string]
    $subscriptionID = '',

    [Parameter(Mandatory= $True,
                HelpMessage='Enter the Azure Data Center Region to deploy your resources')]
    [string]
    $location = '',

    [Parameter(Mandatory= $True,
                HelpMessage='Enter the admin Username for Virtual Machine')]
    [string]
    $adminUserName = '',

    [Parameter(Mandatory= $True,
                HelpMessage='Enter the admin password for Virtual Machine')]
    [string]
    $adminPwd = ''
)

Write-Host "Log in to Azure.....`r`n" -ForegroundColor Yellow
az login

az account set --subscription $subscriptionID
Write-Host "Switched subscription to '$subscriptionID' `r`n" -ForegroundColor Yellow

Write-Host "Started deploying Blockchain and NFT Service resources.....`r`n" -ForegroundColor Yellow
$deploymentResult = az deployment sub create --template-file .\main.bicep -l $location -n 'BlockchainNFTServiceDeploy' -p adminUserName=$adminUserName adminPwd=$adminPwd
$joinedString = $deploymentResult -join "" 
$jsonString = ConvertFrom-Json $joinedString

$vmDomainName  = $jsonString.properties.outputs.pipDomainName.value
$rgName = $jsonString.properties.outputs.blockchainRgName.value
$vmDnsUrl = 'http://' +  $vmDomainName + ':8545/'
$kubernetesName  = $jsonString.properties.outputs.aksName.value
$containerRegistryName  = $jsonString.properties.outputs.acrName.value
$cosmosName  = $jsonString.properties.outputs.cosmosName.value
$akvUrl  = $jsonString.properties.outputs.akvUrl.value
$resourcegroupName = $jsonString.properties.outputs.nftRgName.value
$nftUserIdentityClientId = $jsonString.properties.outputs.nftUserIdentityClientId.value

Write-Host "--------------------------------------------`r`n" -ForegroundColor White
Write-Host "Deployment output: `r`n" -ForegroundColor White
Write-Host "Subscription Id: $subscriptionID `r`n" -ForegroundColor Yellow
Write-Host "NFT service resource group: $resourcegroupName `r`n" -ForegroundColor Yellow
Write-Host "Kubernetes Account: $kubernetesName" -ForegroundColor Yellow
Write-Host "Blockchain resource group created: $rgName" -ForegroundColor Yellow
Write-Host "Blockchain vm dns Url created: $vmDnsUrl `r`n" -ForegroundColor Yellow
Write-Host "Container registry: $containerRegistryName" -ForegroundColor Yellow
Write-Host "Cosmos DB account: $cosmosName" -ForegroundColor Yellow
Write-Host "KeyVault Url: $akvUrl" -ForegroundColor Yellow 
Write-Host "User Assigned Identity Client Id: $nftUserIdentityClientId " -ForegroundColor Yellow
Write-Host "--------------------------------------------`r`n" -ForegroundColor White

# Update the endpoint URL in console app appsetting 
((Get-Content -path ..\..\..\src\Solutions.NFT.ConsoleApp\appsettings.json -Raw) -replace '{BlockchainTxEndpointURL}', $vmDnsUrl) | Set-Content -Path ..\..\..\src\Solutions.NFT.ConsoleApp\appsettings.json
Write-Host "Updated appsettings.json for Console App (client).....`r`n" 


# Update the settings in console app setting 
((Get-Content -path ..\..\..\src\Solutions.TokenService.API\appsettings.json -Raw) -replace '{SubscriptionId}', $subscriptionID) | Set-Content -Path ..\..\..\src\Solutions.TokenService.API\appsettings.json
((Get-Content -path ..\..\..\src\Solutions.TokenService.API\appsettings.json -Raw) -replace '{ResourceGroupName}', $resourcegroupName) | Set-Content -Path ..\..\..\src\Solutions.TokenService.API\appsettings.json
((Get-Content -path ..\..\..\src\Solutions.TokenService.API\appsettings.json -Raw) -replace '{DatabaseAccountName}', $cosmosName) | Set-Content -Path ..\..\..\src\Solutions.TokenService.API\appsettings.json
((Get-Content -path ..\..\..\src\Solutions.TokenService.API\appsettings.json -Raw) -replace '{ManagedIdentityId}', $nftUserIdentityClientId) | Set-Content -Path ..\..\..\src\Solutions.TokenService.API\appsettings.json
((Get-Content -path ..\..\..\src\Solutions.TokenService.API\appsettings.json -Raw) -replace '{KeyVaultUrl}', $akvUrl) | Set-Content -Path ..\..\..\src\Solutions.TokenService.API\appsettings.json
Write-Host "Updated appsettings.json for Token Service.....`r`n" 

Write-Host "All resources are deployed successfully.....`r`n" -ForegroundColor Green