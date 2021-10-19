# LoginServiceInRedisAndDotnetFive
Runs 3 executables(Frontend, Proxy and Backend) to authenticate a user.

To run with a real redis server, please modify the `redisConnectionString` value in {Service}/Config/{Service}.Config.json files according to your local environment.

I am using Ubuntu WSL with podman installed.

Cheatsheet commands:
1)`podman build -t loginproxy .` -> Call this method inside Login.Proxy folder. This will use the Dockerfile inside that directory to create an image of Proxy service with name loginproxy.

2)`podman run -d --network redis localhost/loginbeservice` -> Run 1st command in Login.Backend folder with respective tag then run this command. This command will run Backend Service in detached mode. 

3)`podman run --network redis -it localhost/loginfeservice -a` -> Run 1st command in Login.Frontend folder with respective tag then run this command. This will start a container in redis network with interactive terminal session. Also passes argument "-a" to the process inside the container. 

