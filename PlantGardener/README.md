# Plant-Gardener

Plant-Gardener is a service to upload images to targentmanager for Vufora cloud. The progam is using the the custom [VWS authentication](https://library.vuforia.com/web-api/vuforia-web-api-authentication#vws-authentication)

## Installation

Install `virtualenv`

    pip install virtualenv

Create enviroment called `venv`

    python -m venv venv

Activate the enviroment:

    # Windows
    ./venv/Scripts/activate

    # Linux
    source ./venv/bin/activate

Install python libs

    pip install -r requirements.txt

### Ubuntu WSL

Install OpenCV dependencies:

    apt-get update && apt-get install -y python3-opencv

### Windows Powershell

Set powershell modes for python venv execution:     
    
    # forbid .ps1 execution 
    Set-ExecutionPolicy Restricted -force 
    
    # allow .ps1 execution 
    Set-ExecutionPolicy Unrestricted -force 