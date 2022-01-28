#login azure
Write-Host "Log in to Azure"
az Login

$subscriptionId = Read-Host "subscription Id"
$resourcegroupName = Read-Host "resource group name"
$containerRegistryName = Read-Host "container registry name"
$kubernetesName = Read-Host "kubernetes account name"

az account set --subscription $subscriptionId

$resourceGroup = az group exists -n $resourcegroupName
if ($resourceGroup -eq $false) {
    throw "The Resource group '$resourcegroupName' is not exist`r`n Please check resource name and try it again"
}

Write-Host "Setup Azure Container Registry....."

az acr update --name $containerRegistryName --admin-enabled true

Write-Host "Retrieving configuration setting in Container Registry..."

$acrList = az acr list | Where-Object { $_ -match $containerRegistryName + ".*" }
$loginServer = $acrList[1].Split(":")[1].Replace('"', '').Replace(',', '').Replace(' ', '')
$registryName = $acrList[2].Split(":")[1].Replace('"', '').Replace(',', '').Replace(' ', '')

$userName = $registryName
$password = ( az acr credential show --name $userName | ConvertFrom-Json).passwords.value.Split(" ")[1] 

Write-Host "..... loginServer: '$loginServer'"
Write-Host "..... registryName: '$registryName'"
Write-Host "..... userName: '$userName'"
Write-Host "..... userPassword: '$password'"

Write-Host "Setup Azure Kubernetes Service and Azure Container Service..."

az aks update -n $kubernetesName -g $resourcegroupName --enable-managed-identity

az aks update -n $kubernetesName -g $resourcegroupName --attach-acr $containerRegistryName

Write-Host "Build container image and push to Azure container registry...."

#Build and Containerizing then Push to Azure Container Registry
Set-location "..\..\src"

docker build -f .\Solutions.TokenService.API\Dockerfile --rm -t 'tokenservicehub/acrtokenservice' .

docker tag 'tokenservicehub/acrtokenservice' "$containerRegistryName.azurecr.io/tokenservicehub/acrtokenservice"

Write-Host "Log in to ACR `r`n"
docker login "$containerRegistryName.azurecr.io" -u $userName -p $password

Write-Host "Push Image to ACR`r`n"
docker push "$containerRegistryName.azurecr.io/tokenservicehub/acrtokenservice"

Set-Location ..

Write-Host "Set up AKS and Deploy Applications from Azure Container Registry...."
# Set Kubernetes context
az aks get-credentials -g $resourceGroupName -n $kubernetesName

Write-Host "Preparing Kubernetes Deployment.....`r`n"
# Set up Deployment yaml file to deploy APIs
((Get-Content -path manifests\kubernetes-deployment.yaml.template -Raw) -replace '{acrname}', $containerRegistryName) | Set-Content -Path manifests\kubernetes-deployment.yaml

Write-Host "Deploy Services from ACR to Kubernetes.....`r`n"
kubectl create ns tokenservicehub

# #Deploy Containers to Kubernetes
kubectl apply -f manifests\kubernetes-deployment.yaml --namespace tokenservicehub

Write-Host "Application deployment has been finished.....`r`n"

Write-Host "Update the console Application tokenservice URL"
$ExternalIP =""

$ExternalIP =""
while($true)
{
    
    $ExternalIP =  (((kubectl get services -n tokenservicehub)[1])-split "\s+")[3]
    if(!$ExternalIP  -or $ExternalIP -like "*pending*")
    {
        Start-Sleep -Seconds 5
    }
    else
    {
        break
    }
    
}

$NFTServiceURL = "http://$ExternalIP"

((Get-Content -path .\src\Solutions.NFT.ConsoleApp\appsettings.json -Raw) -replace '{NFTServiceURL}', $NFTServiceURL) | Set-Content -Path .\src\Solutions.NFT.ConsoleApp\appsettings.json

Write-Host "IP Address for NFT token service: $ExternalIP `r`n"

Write-Host "Token Service deployed successfully.....`r`n" -ForegroundColor Green