# Application Documentation

This document provides guidance on setting up the application, using its Swagger API, and fetching data. Follow the steps below to ensure you’re able to efficiently work with the application.

---

## **Setting Up the Application**

1. **Clone the Repository:**
   ```bash
   cd Java
   ```

2. **Run the Application:**
    - Using **Maven**:
      ```bash
      mvn clean spring-boot:run
      ```
    - Using **IntelliJ IDEA**:
        1. Open the project in IntelliJ IDEA.
        2. Navigate to the main class file (the one annotated with `@SpringBootApplication`).
        3. Right-click and select **Run**.
      ```

## **Accessing the Swagger API**

Swagger provides interactive documentation to test and explore available endpoints.

1. **Access Swagger UI:**
   Open your browser and navigate to:
   ```
   http://localhost:8080/swagger-ui.html
   ```

2. **Explore API Endpoints:**
    you can find all the endpoints in the swagger url and you can try them as well

---

## **Fetching Data**

To fetch data from the application’s REST API, you can use tools like **Postman**, **cURL**, or programming libraries (e.g., `axios` for JavaScript or `RestTemplate` for Java).

### **Example: Using cURL**

1. **Perform a GET Request:**
   ```bash
   curl -X GET http://localhost:<port>/api/resource
   ```
   Replace `/api/resource` with the actual endpoint.

2. **Perform a POST Request:**
   ```bash
   curl -X POST http://localhost:<port>/api/resource \
   -H "Content-Type: application/json" \
   -d '{
       "key": "value"
   }'
   ```

3. **Response Handling:**
    - A successful response typically contains `200 OK` or similar status codes.
    - Any validation errors or server-side issues will return responses like `4XX` or `5XX`, so review and debug accordingly.

---

## **Testing the API**

To validate the workings of APIs:
1. Use **Postman** or **Swagger UI** to test various endpoints.
2. Check status codes and returned responses for troubleshooting.

---

# API Endpoints Documentation

This document contains detailed information about the available API endpoints for the application. The controllers and their respective endpoints are listed below.

---

## **Vehicle Controller**

### 1. **GET** `/api/v1/vehicle/{id}`
- Retrieve details of a specific vehicle by its ID.

### 2. **PUT** `/api/v1/vehicle/{id}`
- Update information for a specific vehicle.

### 3. **DELETE** `/api/v1/vehicle/{id}`
- Delete a specific vehicle by its ID.

### 4. **GET** `/api/v1/vehicle`
- Retrieve a list of all vehicles.

### 5. **POST** `/api/v1/vehicle`
- Create a new vehicle.

### 6. **POST** `/api/v1/vehicle/{id}/toll-dates`
- Add toll dates for a specific vehicle by its ID.

---

## **Vehicle Type Controller**

### 1. **GET** `/api/v1/vehicle-type/{id}`
- Retrieve details of a specific vehicle type by its ID.

### 2. **PUT** `/api/v1/vehicle-type/{id}`
- Update information for a specific vehicle type.

### 3. **DELETE** `/api/v1/vehicle-type/{id}`
- Delete a specific vehicle type by its ID.

### 4. **GET** `/api/v1/vehicle-type`
- Retrieve a list of all vehicle types.

### 5. **POST** `/api/v1/vehicle-type`
- Create a new vehicle type.

---

## **Rate Controller**

### 1. **GET** `/api/v1/rate/{id}`
- Retrieve details of a specific rate by its ID.

### 2. **PUT** `/api/v1/rate/{id}`
- Update a specific rate.

### 3. **DELETE** `/api/v1/rate/{id}`
- Delete a specific rate by its ID.

### 4. **GET** `/api/v1/rate`
- Retrieve a list of all available rates.

### 5. **POST** `/api/v1/rate`
- Create a new rate.

---

## **City Controller**

### 1. **GET** `/api/v1/city/{id}`
- Retrieve information about a specific city by its ID.

### 2. **PUT** `/api/v1/city/{id}`
- Update information for a specific city.

### 3. **DELETE** `/api/v1/city/{id}`
- Delete a specific city by its ID.

### 4. **GET** `/api/v1/city`
- Retrieve a list of all cities.

### 5. **POST** `/api/v1/city`
- Create a new city.

---

## **Toll Calculator Controller**

### 1. **POST** `/api/v1/toll`
- Calculate toll charges based on the vehicle registration number and the city code.
- You can check the available vehicle registration number by the GET request to this endpoint /api/v1/vehicle
- To get the available city code you can make GET request to this endpoint /api/v1/city 

### 2. **POST** `/api/v1/toll/vehicle-type`
- Calculate toll charges for a specific vehicle type in a city at specified toll dates.
- To check available vehicle type you can make a GET request to this endpoint /api/v1/vehicle-type

Finally How you create the body and all the parameters just look at the swagger url
   ```
   http://localhost:8080/swagger-ui.html
   ```

---

## **Additional Notes**

- Ensure you have the required tools installed:
    - **Java 17+**
    - **Maven 3.6+**
    - A database like **PostgreSQL**, **MySQL**, or **H2** (as per configuration).
- The application uses **Lombok**, so ensure your IDE supports it (install the Lombok plugin in IntelliJ IDEA if needed).

For further assistance or documentation, feel free to consult the development team or refer to the project README file.

---