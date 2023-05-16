#!/bin/bash

docker rm `sudo docker ps -a -q`
docker rmi webapp --force
docker build --tag webapp .
docker run --rm --name webapp --publish 5000:5000 webapp
