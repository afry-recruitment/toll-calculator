# Toll Fee Calculator API Service 1.0

This README file provides instructions on how to set up and run a toll-calculator API service. It also includes
information on how to invoke the API and details about the CI pipeline.

## Project Setup Instructions

To set up the project, follow these steps:

1. Clone the repository to your local machine.
2. Create a virtual environment `python3 -m venv venv`
3. Activate the virtual environment `source venv/bin/activate`.
4. Install the required dependencies `pip install -r requirements.txt`.

## Database Setup Instructions

To set up the database, follow these steps:

1. Install a database server by running the docker-compose.yml file as below.
    ```
   version: "3"
   services:
       mongo_db:
          image: mongo:4.2.3
          restart: always
          environment:
            MONGO_INITDB_ROOT_USERNAME: <database username>
            MONGO_INITDB_ROOT_PASSWORD: <database password>
            MONGO_INITDB_DATABASE: <database name>
          volumes:
            - mongo-data:/data/db
            - ./init-scripts:/docker-entrypoint-initdb.d
          ports:
            - "27017:27017"
   volumes:
      mongo-data:
    ```
2. Initial data will be automatically populate in to the database. `init-scripts` folder.
3. Update `env` file in the root directory of the project and add the following variables with your database connection
   details:
    ```
   MONGO_DATABASE=<database name>
   MONGO_HOST=localhost
   MONGO_PORT=27017
   MONGO_USERNAME=<database username>
   MONGO_PASSWORD=<database password>
    ```

## Project Run Instructions

To run the project, follow these steps:

1. Activate the virtual environment if it is not already activated.
2. Run the command `python app.py` to start the server by providing the environment variables in env file
3. The API Swagger will be available at `http://localhost:3000`.

## Running the Unit Tests

To run the unit tests, follow these steps:

1. Activate the virtual environment if it is not already activated.
2. Run the command `python -m pytest  tests/` to run the tests.
3. The test results will be displayed in the terminal.

## CI Pipeline Details

The project includes a CI pipeline that runs automated tests, build and push docker images to the docker hub

Pipeline: [GitHub CI Pipeline](https://github.com/padmasankha/toll-calculator/actions)

1. On every push to the `main` branch, the pipeline runs the automated tests to ensure that the service is working
   correctly.
2. If the tests pass, the pipeline build and push the docker images
   to [docker hub](https://hub.docker.com/r/padmasankha/toll-fee-api-service).

## API Invocation Workflow

The API service provides a three main API to complete the workflow. In addition to that `/api/echo` that returns a
greeting
message to check the liveness of the API.

All the APIs and the payloads are mention in the Swagger Doc [http://localhost:3000](http://localhost:3000)

#### 1. Get all Vehicle Types

```http
  GET /api/v1/vehicle-type
```

#### 2. Vehicle checked in

```http
  POST /api/v1/vehicle-in
```

| Parameter      | Type     | Description                                      |
|:---------------|:---------|:-------------------------------------------------|
| `license_no`   | `string` | **Required**. Vehicle License No                 |
| `vehicle_type` | `string` | **Required**. This Value Return from the 1st API |
| `vehicle_code` | `string` | **Required**. This Value Return from the 1st API |
| `is_free`      | `string` | **Required**. This Value Return from the 1st API |

#### 3. Vehicle checked exit

```http
  POST /api/v1/vehicle-exit
```

| Parameter    | Type     | Description                      |
|:-------------|:---------|:---------------------------------|
| `license_no` | `string` | **Required**. Vehicle License No |