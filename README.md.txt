E-Commerce Microservices:
This project consists of multiple microservices designed to handle different functionalities of an e-commerce platform. Below are brief descriptions of each service:

Microservices Overview:-
UserAuthService:
The User service manages user authentication, registration, and profile management. It provides endpoints for user login, registration, fetching user details, updating user information, and deleting user accounts.

Product Service:
The Product service handles product management, including creation, retrieval, updating, and deletion of products. It also supports search and filtering functionality for product retrieval based on various criteria.

Product Details Service:
The Product Details service stores additional information about products, such as descriptions, specifications, and images. It complements the Product service by providing detailed information for each product.

Order Service:
The Order service facilitates order management, allowing users to place, view, modify, and cancel orders. It tracks order statuses and provides order history retrieval functionalities.

Cart Service:
The Cart service manages user shopping carts, enabling users to add, view, update, and remove items from their carts. It integrates with the Order service to convert carts into orders during checkout.

Technologies Used:
ASP.NET Core: Used to build the RESTful APIs for each microservice.
Docker: Used for containerization to deploy microservices as isolated, lightweight containers.
Swagger: Integrated with each microservice to provide API documentation and testing capabilities.
JWT Authentication: Implemented for securing the APIs, allowing only authorized users to access certain endpoints.

Docker Hub Images:
UserAuthService Docker Image - https://hub.docker.com/layers/harshitojha08/microservices/user-service/images/sha256-0736ae9091a7a4e85c718668d0b135378c70257fcba3e903cc25650d11a49456?context=repo .
Product Service Docker Image - https://hub.docker.com/layers/harshitojha08/microservices/product-service/images/sha256-1cf14147a04213ed71add5761acc82bc6e4dcf9b37f996dcd8a8898789ebd82a?context=repo .
Product Details Service Docker Image - https://hub.docker.com/layers/harshitojha08/microservices/productDetails-service/images/sha256-e409321453c6e9fb68f70203f8bd17fce6df453836b97c3e9784a0ce9c2b539d?context=repo .
Order Service Docker Image - https://hub.docker.com/layers/harshitojha08/microservices/order-service/images/sha256-21eb5b8d5aebf208181529e4c2d96d85526102c52f141078d0611f0af70e1da5?context=repo .
Cart Service Docker Image - https://hub.docker.com/layers/harshitojha08/microservices/cart-service/images/sha256-aba8162c6b7e87101806f26703afe8ddbcf49165a90ab4aea522ef2f569af525?context=repo .

Additional Resources:
Postman Collections - Postman collections for testing the API endpoints is available in the folder named "Postman".

How to Run:
Clone the repository to your local machine.
Navigate to the root directory of each microservice.
Run docker-compose up to build and start the Docker containers for each microservice.
Once the containers are running, you can access the APIs using the provided endpoints.