# Bluetooth-Attraction-Guide-HSP
A github repository to store my Honours Stage Project, a Bluetooth Attraction Guide

# Introduction

Welcome to my honour stage project, in this readme we will only be going though how to deploy the current code as well as an explanation on what all the different projects do. This project is not simple at all to deploy and requires a investment of a live sever (or servers) as well as some form of BLE beacons, and a mobile device (Preferably an android one).

# TestData Folder

Here there is a test data folder, this is data that was used in testing and development of the project, the "Raw" folder has content of the raw data collection, proses is just data that is paired with the AI's predicted result, and the "AI models" folder has any AI models that where built and used in the project.

# Docker Comprise

For the WebAPI and the WebClient, both can be hosted onto a server using docker comprise, the file presented here can set up a Nigex revers proxy server with a db to manage connections and will also create a second db for the WebAPI and WebClient to use. If you do decide to use the docker compose file, then please be aware you will have to log into the Nigex web portal to set up routing and SSL connections. And prior knowledge on how docker comprise works will be needed. 

# Not using Docker

If you wish to host this project in any other way, it would recommend to use 3 separate servers if you can, one for database hosting, one for the WebClient and one for the WebAPI.

# MobileClient

The MobileClient can be also called the dev client, this is used to gather beacon data for testing with a ai module, please make sure to replace all references "jessicawylde.co.uk" with the domain you set up with the webAPI.

# MoblieUserClient

Same as the mobile client, please make sure you remove "jessicawylde.co.uk" and replace it yourself with your API URL. This client acts as the main tracking app, please remember to also change X_Value and Y_Value with your own trained AI.
