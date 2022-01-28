## Resource Deployment
The [PowerShell script](./Bicep/resourcedeployment.ps1) can be used to provision the Azure resources required to deploy this Non-fungibleToken Solution Accelerator. 
You may skip this section if you prefer to provision your Azure resources via the Azure Portal, using the Azure Resource Manager(ARM) Template provided, or the Deploy to Azure button on the main documentation page.

**The PowerShell script will provision the following resources to your Azure subscription:**

### Blockchain Resources:
- Azure Virtual Machine
- Azure Virtual Network
- Azure Public IP Address
- Azure Network Security Group
- Azure Network Interface
- Azure Disk

### NFT Service Resources:
- Azure Container Registry
- Azure Kubernetes Service
- Azure Key Vault
- Azure Cosmos DB Account

## Prerequisites
 1. [Azure Subscription](http://portal.azure.com) - Required to deploy compute resources
 2. [PowerShell 7.1](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-7.1) - Required to run deployment scripts
 3. [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) installed - Required to run deployment scripts
 4. [User Access Administrator](https://docs.microsoft.com/en-us/azure/role-based-access-control/built-in-roles#user-access-administrator) Role - Assigned to the user in Azure Subscription
 5. [PuTTy](https://www.putty.org/) - Required for ssh into VM

Execute the following steps to deploy Azure resources:

### Step 1. Download Files
Clone or download this repository, if you have not already done so.

Check here for more information on [cloning a repository](https://docs.github.com/en/repositories/creating-and-managing-repositories/cloning-a-repository).

### Step 2. Deploy Blockchain Service and NFT Service Resources
1. Run [PowerShell 7.1](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-7.1)

2. Run Change Directory command to Navigate to the Path where resourcedeployment.ps1 is present
    ```console
    PS C:\Users\>CD <directory path>
    ```

3. Run the **resourcedeployment.ps1** with the following parameters:
```.\resourcedeployment.ps1 <SubscriptionId> <location> <AdminName> <AdminPassword>```

    ```
    SubscriptionId : The subscription ID for where you want to manage your resources
    location : Azure Data Center Region where resources will be deployed
    AdminName :  Admin User Name for BlockChain Virtual Machine
    AdminPassword : Admin password for Blockchain Virtual Machine
    ```

    - In case you get the below error for running the PowerShell script:

        ![alt text](/documents/media/resourceDeploymentError.png)

    - To resolve the above issue run the following command:
        ```Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass```

    - Then run ```.\resourcedeployment.ps1 <SubscriptionId> <location> <AdminName> <AdminPassword>```


    - After the completion of the script, check to see that all of the Azure resources deployed successfully. Your resource groups should look similar to the image below.

        ![alt text](/documents/media/Resources.png)

### Step 3. Configure Managed Identity 

**Note: The managed identity name will differ by deployment. Your managed identity will be different. Ex. NFTUserIdentity-XXXXX**

#### Assign Managed Identity to AKS
1. Step into the aks VM scale set

    ![alt text](/documents/media/aksVmScaleSet.png) 

2. Under settings click on the identity

    ![alt text](/documents/media/aksIdentity.png)

3. Click on the User Assigned tab and click on add and select NFTUserIdentity-XXX

    ![alt text](/documents/media/aksAssignIdentity.png)

4. Refresh to confirm identity assignment
 
    ![alt text](/documents/media/aksAssignIdentityConfirm.png)

#### Assign Managed Identity to Key Vault
1. Step into the key vault

    ![alt text](/documents/media/KeyVault.png) 

2. Click on the Access control and click on add and select Add role assignment

    ![alt text](/documents/media/KeyVaultAccess.png)

3. Search for the "Key Vault Crypto Officer" in the search box given. Select the "Key Vault Crypto Officer" role and click next

    ![alt text](/documents/media/KeyVaultAccessAssign.png)

4. Click on the select members and select NFTUserIdentity-XXX

    ![alt text](/documents/media/KeyVaultIdSelect.png)

5. Click on review + assign to add the role assignment

    ![alt text](/documents/media/KeyVaultIdAssign.png)

6. Refresh to confirm role assignment
 
    ![alt text](/documents/media/KeyVaultAccessReview.png)

#### Assign Access Policy to Managed Identity in Key Vault
1. Step into the key vault

    ![alt text](/documents/media/KeyVault.png) 

2. Under settings click on the Access policies and click on add access policy

    ![alt text](/documents/media/KeyVaultAccess.png)

3. Select Key Management in the template. Select the Get, Update, Create, Delete, Verifya, and Sign in the key permissions

    ![alt text](/documents/media/KeyVaultPermissions.png)

4. Click on the none selected and select NFTUserIdentity-XXX

    ![alt text](/documents/media/KeyVaultIdentitySelect.png)

5. Click on add to add access policy

    ![alt text](/documents/media/KeyVaultIdentityAdd.png)

6. Review the permissions and click on save to commit your changes

    ![alt text](/documents/media/KeyVaultIdentityAddSave.png)

7. Refresh to confirm access policy
 
    ![alt text](/documents/media/KeyVaultIdentityVerify.png)

#### Assign Managed Identity to Cosmos DB
1. Step into the Cosmos DB account

    ![alt text](/documents/media/CosmosDB.png) 

2. Click on the Access control and click on add and select Add role assignment

    ![alt text](/documents/media/CosmosDBAccess.png)

3. Search for the "DocumentDB Account Contributor" in the search box given. Select the "DocumentDB Account Contributor" role and click next

    ![alt text](/documents/media/CosmosDBAccessSelect.png)

4. Click on the select members and select NFTUserIdentity-XXX

    ![alt text](/documents/media/CosmosDBAccessSelectID.png)

5. Click on review + assign to add the role assignment

    ![alt text](/documents/media/CosmosDbIdReveiw.png)

6. Refresh to confirm role assignment
 
    ![alt text](/documents/media/CosmosDbIdVerify.png)

**This script will also update appsettings.json with values required to communicate with these resources.**

**Remember to write down all of the output values printed on the screen. These are required in the next step (while deploying Token service).**

**You've successfully deployed all the resources!**

For next step, go to [Quorum Configuration](/deployment/ARMTemplates/QuorumConfiguration.md).