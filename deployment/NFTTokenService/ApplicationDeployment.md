# Application Deployment

## Prerequisites
In order to successfully deploy the token service, you will need to have deployed the following resources.
 
 1. Azure Container Registry
 2. Azure Kubernetes Service
 3. Azure Key Vault
 4. Azure Cosmos DB Account
 5. Docker application needs to be in running

If these are not available, please follow the [resource deployment](../ARMTemplates/ResourceDeployment.md) steps. 

## Deploy Token Service on Azure Kubernetes Service

### Execute the PowerShell deployment script
1. Run [PowerShell 7.1](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-7.1)
2. Run Change Directory command to Navigate to the Path where deployapplications.ps1 is present
    ```console
    PS C:\Users\>cd <driectory path>
    ```
3. Run the .\deployapplications.ps1
    ```console
    Powershell.exe -executionpolicy remotesigned -File .\deployapplications.ps1
    ```
4. Accept the log-in request through your browser

5. Enter the following detail in console when prompted which are obtained from previous script(resourcedeployment.ps1) output
    ```
    SubscriptionId : The Subscription ID for where you want to manage your resources
    ResourcegroupName : Resource group name where the resources are deployed
    ContainerRegistryName :  Container registry name where the docker image will be uploaded
    kubernetesName : Kubernetes cluster name
    ```

6. After the successful execution of script, the Token service will be deployed.

    **Note: This script will also update appsettings.json for the sample console service with values required to communicate with this service.**

    **You've successfully deployed the Token service!**


Next, please go to [**Solution Testing**](/documents/NFTSampleConsoleApp.md).

---

## Description of the deployment script (Optional)
1. Log in to the Azure portal

    ```
    az login
    ```
2. Set the Azure account Subscription ID

    ```
    az account set --subscription mysubscriptionid
    ```
3. Setup Azure Container Registry

    ```
    az acr update --name mycontainerregistryname  --admin-enabled true
    ```
4. Retrieving Configuration Setting in Container Registry

    ```
    $acrList = az acr list | Where-Object { $_ -match $containerRegistryName + ".*" }
    $loginServer = $acrList[1].Split(":")[1].Replace('"', '').Replace(',', '').Replace(' ', '')
    $registryName = $acrList[2].Split(":")[1].Replace('"', '').Replace(',', '').Replace(' ', '')

    $userName = $registryName
    $password = ( az acr credential show --name $userName | ConvertFrom-Json).passwords.value.Split(" ")[1]
    ```
5. Setup Azure Kubernetes Service and Azure Container Service

    ```
    az aks update -n mykubernetesservicename -g myresourcegroupname --attach-acr mycontainerregistryname
    ```
6. Build the Container and push the Azure Container Registry

    ```
    Set-location <Replace Source code path>

    docker build -f <Replace docker file path> --rm -t <Define token service name for container registry>.

    docker tag <Replace token service name for container registry> <mycontainerregistryname.azurecr.io + token service name defined above>

    docker login <mycontainerregistryname.azurecr.io > -u $userName -p $password

    docker push "$mycontainerregistryname.azurecr.io/tokenservicehub/acrtokenservice"

    Set-Location ..
    ```
7. Set up Azure Kubernetes Service and Deploy Applications from Azure Container Registry

    ```
    az aks get-credentials -g myresourceGroupName -n mykubernetesName
    ```
8. Set up Deployment yaml file to deploy APIs

    ```
    ((Get-Content -path manifests\kubernetes-deployment.yaml.template -Raw) -replace '{acrname}', mycontainerregistryname) | Set-Content -Path manifests\kubernetes-deployment.yaml
    ```
9. Deploy Services from Azure Container Registry to Kubernetes, and create namespace

    ```
    kubectl create ns tokenservicehub
    ```
10. Deploy Containers to Kubernetes

    ```
    kubectl apply -f manifests\kubernetes-deployment.yaml --namespace tokenservicehub
    ```