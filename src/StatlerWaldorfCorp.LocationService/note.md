
# Setting up Communication Between Containers in Docker

This guide explains how to establish communication between a .NET application (specifically, `locationservice`), environment variables, and a local PostgreSQL Docker server.

Below is a README.md documentation created for **page 78-80** clarification.

## Prerequisites

- Docker installed on your local machine.
- Basic understanding of Docker concepts such as containers, images, and port mapping.


### Docker Concepts

#### Port Mapping

- **Syntax**: The format used for port mapping in Docker is `hostmachine:container`.
- **Explanation**: This syntax indicates that a port on the host machine is mapped to a port inside the Docker container. For instance, `5432:5432` maps port 5432 on the host machine to port 5432 inside the container.
- **Purpose**: Port mapping allows external systems to communicate with services running inside Docker containers by exposing container ports to the host machine.

#### Container Linking

#### Container Linking and Hostname Configuration

- When linking containers in Docker, any valid hostname can be specified, including "abc" or any other custom name.
- Docker automatically sets up hostname resolution within linked containers.
- The hostname specified in environment variables or Docker commands points to the linked container's hostname, not the local PC or the image itself.
- Container names remain consistent unless explicitly changed when running Docker commands.


- **Syntax**: Container linking is specified as `containername:alias`.
- **Explanation**: This syntax establishes a connection between containers, allowing them to communicate with each other. The `containername` refers to the name of the container being linked, while the `alias` is a user-defined identifier for referencing the linked container.
- **Purpose**: Container linking enables services running in different containers to interact with each other securely and efficiently.

#### Combining Both

- **Syntax**: When combining port mappings and container links, the format remains `hostmachine:container`.
- **Explanation**: This format ensures consistency in specifying connections between the host machine, Docker containers, and linked containers. For example, `docker run -p 8080:8888 --link some-container:alias ...` maps port 8080 on the host machine to port 8888 inside the container and links the `some-container` container with the alias `alias`.
- **Purpose**: Combining port mappings and container links provides a comprehensive approach to configuring communication between Docker containers and external systems.

#### Setting up Communication and Environment Variables

1. **Exporting Environment Variables**:
   - Set environment variables using the `export` command in the terminal or define them in Dockerfile or docker-compose.yml files.
   - Follow standard conventions for naming and formatting environment variable values.

   Example:
   ```bash
    export POSTGRES_CSTR="Host=abc;Username=integrator;Password=inteword;Database=locationservice;Port=5432"
   ```

2. **Running Docker Containers**:
   - Use Docker commands to start containers and link them together as needed.
   - Specify custom hostnames for linking containers and ensure consistent naming conventions for environment variables.

   Example:
   ```bash
   docker run -p 5432:5432 --name some-postgres -e POSTGRES_PASSWORD=inteword -e POSTGRES_USER=integrator -e POSTGRES_DB=locationservice -d postgres
   ```

   ```bash
    docker run -p 5000:5000 --link some-postgres:abc -e TRANSIENT=false -e PORT=5000 -e POSTGRES_CSTR dotnetcoreservices/locationservice:latest
   ```
   - Explanation of the command components:

   - -p 5000:5000: Maps port 5000 on the host machine to port 5000 inside the container.
   - --link some-postgres:abc: Links the container to another named "some-postgres" with the alias "abc".
   - -e flags: Sets environment variables for configuration.
   - dotnetcoreservices/locationservice:latest: Specifies the Docker image and tag for creating the container.

### Using Environment Variables

#### Usage of Environment Variables

- Environment variables play a crucial role in dynamically configuring Docker containers.
- Standard conventions for naming environment variables in .NET applications include descriptive names, uppercase letters, and separators such as underscores or hyphens.
- Examples of environment variable names following conventions:
  - `POSTGRES_CSTR`: PostgreSQL connection string.
  - `SMTP_SERVER`: SMTP server address for sending emails.
  - `REDIS_CACHE`: Address of a Redis cache server.
  - `MAX_RETRIES`: Maximum number of retries for a specific operation.

- **Purpose**: Environment variables play a crucial role in configuring Docker containers dynamically.
- **Set Up**: Environment variables can be set using the `export` command in the terminal or defined in Dockerfile or docker-compose.yml files.
- **Standard Conventions**: Follow standard conventions for naming environment variables, including descriptive names, uppercase letters, and separators such as underscores or hyphens.
- **Example**: 
  ```bash
  export POSTGRES_CSTR="Host=postgres;Username=integrator;Password=inteword;Database=locationservice;Port=5432"
  ```
- **Purpose**: The `POSTGRES_CSTR` environment variable contains the PostgreSQL connection string, enabling the .NET application to establish a connection with the PostgreSQL database.
- **Reading Environment Variables**: During startup, the .NET application Docker service automatically reads environment variables set in the environment where it's running. These environment variables can be accessed programmatically within the application.
- **Usage in Code**: In your .NET application, you can retrieve the values of environment variables and use them to configure application settings. This typically occurs during the application's startup logic, where configuration settings are read and applied accordingly.

By ensuring that the necessary environment variables are set and accessible during startup, your .NET application Docker service can dynamically configure its settings, including database connections and other runtime parameters, based on the environment where it's deployed.



### Running Docker Containers

1. **Run PostgreSQL Docker Container**:
   - **Explanation**: The command `docker run` initializes a Docker container based on the PostgreSQL image pulled from the Docker Hub. 
   - **Environment Setup**: Environment variables such as `POSTGRES_PASSWORD`, `POSTGRES_USER`, and `POSTGRES_DB` are configured to set up the PostgreSQL server within the container.
   - **Port Exposure**: Port `5432` is exposed on the host machine to enable external systems to communicate with the PostgreSQL server running inside the container.

   ```bash
   docker run -p 5432:5432 --name some-postgres -e POSTGRES_PASSWORD=inteword -e POSTGRES_USER=integrator -e POSTGRES_DB=locationservice -d postgres
   ```

2. **Link Containers and Run `locationservice` Container**:
   - **Explanation**: This command links the `locationservice` container to the PostgreSQL container with the alias `postgres` and passes necessary environment variables to the `locationservice` container.
   - **Purpose**: Container linking ensures seamless communication between the .NET application and the PostgreSQL database.

   ```bash
   docker run -p 5000:5000 --link some-postgres:postgres -e TRANSIENT=false -e PORT=5000 -e POSTGRES_CSTR dotnetcoreservices/locationservice:latest
   ```

These instructions provide a comprehensive overview of setting up communication between Docker containers, configuring environment variables, and running Docker containers for your .NET application and PostgreSQL database. 
