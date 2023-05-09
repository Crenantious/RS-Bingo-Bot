# RSBingoBot

## About this project

TODO

This repository contains the source code for the RSBingoBot application. The application is containerized using Docker.

## Prerequisites

Before you proceed, make sure you have the following software installed on your machine:

1. [Docker](https://www.docker.com/products/docker-desktop): Download and install the latest version of Docker Desktop for your operating system.

## Building the Docker Image

To build the Docker image for the RSBingoBot application, follow these steps:

1. Open a terminal (Command Prompt, PowerShell, or a Unix-based terminal) and navigate to the root folder of the RSBingoBot repository where the `Dockerfile` is located.

2. Run the following command to build the Docker image with the name "rsbingobot" and version tag "1.X.X":

`docker build -t rsbingobot:1.X.X .`

## Running the Application in a Docker Container

`docker run -it --name my_rsbingobot_container rsbingobot:1.X.X`