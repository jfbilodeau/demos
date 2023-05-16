param location string = resourceGroup().location

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: 'teststoragejfb'
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
}


//az deployment group create --name TestDeployment --resource-group Test --template-file .\Test.bicep --parameters location=canadacentral
