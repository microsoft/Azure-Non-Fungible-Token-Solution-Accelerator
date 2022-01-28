# Install the Quorum Dev Quickstart On the VM
1. Go to the blockchain-XXXXX resource group created in the resource deployment step.
    
    ![alt text](/documents/media/BlockchainVM.png)
    

2. Click on the vmXXXX and copy the public IP address.
    
    ![alt text](/documents/media/BlockChainVmPip.png)


3. Open the Putty. Enter the copied IP in the box. Click open to ssh into the vmXXXX.
    
    ![alt text](/documents/media/PuttyIpSSH.png)
    

    ![alt text](/documents/media/PuttyAccept.png)


4. ssh into the VM with credentials provided while running the PowerShell script to create Azure resources.
    
    ![alt text](/documents/media/SSHVM.png)
    

    ![alt text](/documents/media/RunFile.png)
    

5. Run the below command at the user's home directory and select appropriate options: 
`./run.sh`

    ![alt text](/documents/media/QuorumDeploy.png)


6. Navigate to quorum-test-network directory or the directory name you have given, and then execute `./run.sh` present in this directory. This will start the GoQorum service.

**You've successfully deployed the GoQorum service!**

For next step, go to [Token Service Deployment](/deployment/NFTTokenService/ApplicationDeployment.md).