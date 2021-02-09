[CmdletBinding()]
Param(
	[Parameter(Mandatory=$true)]$location,
	[Parameter(Mandatory=$true)]$resourceGroupName
)

# $location = 'East US'
# $resourceGroupName = 'chosen-staging-APIM-RG'


# Create new resource group if not exists.
$rgAvail = Get-AzResourceGroup -Name $resourceGroupName -Location $location -ErrorAction SilentlyContinue
if(!$rgAvail){
    New-AzResourceGroup -Name $resourceGroupName -Location $location
}