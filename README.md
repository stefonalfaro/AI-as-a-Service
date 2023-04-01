# A.I. as a Service
The application has several Models and Controllers that utilize  interfaces to perform various tasks. The primary function of this application is to serve as an API for managing chat completions, fine-tuning, payments, plans, and configurations. By utilizing these interfaces, the application maintains a modular and flexible architecture, allowing for easy modification and extension of its functionalities.

    IChatCompletionService
    IFineTuningService
    IPaymentService
    IPlanService
    IRepository<T>
    IConfigurationService
    IChatHub
    IAuthenticationService
	ICompanyService
	IUserService
	IEmailService
	ITrainingService

## Data Access Layer: Abstractions of SQL Server and CosmosDB
The IRepository<T> interface serves as an abstraction for the data access layer, providing common methods for data manipulation, such as AddAsync, GetByIdAsync, GetAllAsync, UpdateAsync, and DeleteAsync. By using this interface in the services and controllers, we can easily switch between different data storage implementations, such as SQL Server and CosmosDB, by changing the dependency registration in the Startup class.

## State Management: Abstraction for SignalR
Throughout the application, the IHubContext<ChatHub> interface is used to manage the application state in real-time. It is injected into various services to enable communication between the server and the client. The server can push updates to the clients by invoking methods like Clients.All.SendAsync, which allows the front-end to stay in sync with the back-end without requiring manual refreshes.

## Authentication Middleware:
The AuthenticationMiddleware is responsible for handling the authentication process. It checks for the presence of an authentication token in the request headers and validates it. If the token is valid, it sets the current user's identity and allows the request to proceed. If the token is invalid or missing, it returns an appropriate HTTP status code, such as 401 Unauthorized.

# Back-end Developer Documentation

## Setting up Entity Framework
This documentation will guide you through the process of setting up Entity Framework (EF) for the AI as a Service application we have created, ensuring that everything is working correctly.
Prerequisites

Ensure that you have the following installed on your development machine:

    .NET SDK
    Visual Studio or Visual Studio Code

### Steps to Set Up Entity Framework

1. Configure the Connection String: In the appsettings.json file, add a new connection string entry under the "ConnectionStrings" section:
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=YourDatabaseName;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  // ...
}
```
Replace YourDatabaseName with the desired name for your database.

2. Create an initial migration: As we have already created the DbContext and necessary model classes, we need to create our initial migration. Open a terminal, navigate to your project folder, and run the following command:

```
dotnet ef migrations add InitialCreate
```
	
This command will generate a new folder called Migrations with the necessary migration files.

3. Apply the migration: To apply the migration and create the database, run the following command in the terminal:

```	
dotnet ef database update
```

This command will create the database with the schema defined in our DbContext and model classes.

4. Test the setup: To ensure everything is working, you can use the provided API endpoints in the application to perform CRUD operations. For instance, you can use the PlansController's GetPlans endpoint to retrieve all records from the Plans table.

Remember to start your application and use a REST client like Postman to test the API endpoints. If everything is set up correctly, you should be able to interact with the database using the endpoints in the AI as a Service application.

# Front-end Developer Documentation

This documentation will guide you through the process of authorizing API requests and handling real-time updates via SignalR in the AI as a Service application for both Angular and React.
Authentication and Authorization

The AI as a Service application uses JWT (JSON Web Token) for authentication and authorization. To authorize API requests, you need to include the token in the Authorization header of your HTTP requests.

## Authentication and Authorization

### Angular
1. Authentication Service: Create an authentication service to handle user login and token storage.
```
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private apiUrl = 'YOUR_API_BASE_URL';

  constructor(private http: HttpClient) {}

  login(username: string, password: string) {
    return this.http.post(`${this.apiUrl}/auth/login`, { username, password });
  }

  setToken(token: string) {
    localStorage.setItem('token', token);
  }

  getToken() {
    return localStorage.getItem('token');
  }
}
```	

2. HTTP Interceptor: Create an HTTP interceptor to automatically add the Authorization header to your API requests.
```
import { Injectable } from '@angular/core';
import {
  HttpEvent, HttpInterceptor, HttpHandler, HttpRequest
} from '@angular/common/http';
import { AuthenticationService } from './authentication.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthenticationService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const authToken = this.authService.getToken();
    if (authToken) {
      const authReq = req.clone({
        setHeaders: { Authorization: `Bearer ${authToken}` }
      });
      return next.handle(authReq);
    }
    return next.handle(req);
  }
}
```
	
3. Register the HTTP Interceptor: In your app.module.ts, register the interceptor in the providers array:
```
import { AuthInterceptor } from './auth.interceptor';

providers: [
  // ...
  { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
],
```	

### React
1. Authentication Context: Create an authentication context to manage user login state and token storage.
```
import { createContext, useContext, useState } from 'react';

const AuthContext = createContext(null);

export const useAuth = () => useContext(AuthContext);

export const AuthProvider = ({ children }) => {
  const [token, setToken] = useState(localStorage.getItem('token'));

  const login = async (username, password) => {
    const response = await fetch('YOUR_API_BASE_URL/auth/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ username, password })
    });
    const data = await response.json();
    setToken(data.token);
    localStorage.setItem('token', data.token);
  };

  return (
    <AuthContext.Provider value={{ token, login }}>
      {children}
    </AuthContext.Provider>
  );
};
```
	
2. Authorize API Requests: When making API requests, include the Authorization header with the JWT token.
```
import { useAuth } from './auth-context';

const { token } = useAuth();

const fetchData = async () => {
  const response = await fetch('YOUR_API_BASE_URL/api/some-endpoint', {
    headers: { Authorization: `Bearer ${token}` }
  });
  const datajavascript

import { useAuth } from './auth-context';

const { token } = useAuth();

const fetchData = async () => {
  const response = await fetch('YOUR_API_BASE_URL/api/some-endpoint', {
    headers: { Authorization: `Bearer ${token}` }
  });
  const data = await response.json();
  // Process data
};
```

## Real-time Updates with SignalR
The AI as a Service application uses SignalR for real-time updates on object state changes. You will need to connect to the SignalR hub and handle the messages for each object update.

### Angular
1. Install SignalR: Install the @microsoft/signalr package using npm or yarn.
```
npm install @microsoft/signalr
```
	
2. SignalR Service: Create a service to handle the SignalR connection and event listeners.
```
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: signalR.HubConnection;

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('YOUR_API_BASE_URL/chatHub')
      .build();
  }

  connect() {
    this.hubConnection.start()
      .catch(error => console.error('Error connecting to SignalR: ', error));
  }

  on(event: string, callback: (...args: any[]) => void) {
    this.hubConnection.on(event, callback);
  }
}
```
	
3. Use the SignalR Service: Inject the SignalR service in your component and handle the events.
```
import { Component, OnInit } from '@angular/core';
import { SignalRService } from './signalr.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  constructor(private signalRService: SignalRService) {}

  ngOnInit() {
    this.signalRService.connect();
    this.signalRService.on('FineTuneCreated', (fineTuning) => {
      // Handle FineTuneCreated event
    });
    // Add other event listeners as needed
  }
}
```

### React
1. Install SignalR: Install the @microsoft/signalr package using npm or yarn.
```
npm install @microsoft/signalr
```
	
2. SignalR Context: Create a context to handle the SignalR connection and event listeners.

```
import { createContext, useContext, useEffect } from 'react';
import * as signalR from '@microsoft/signalr';

const SignalRContext = createContext(null);

export const useSignalR = () => useContext(SignalRContext);

export const SignalRProvider = ({ children }) => {
  const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl('YOUR_API_BASE_URL/chatHub')
    .build();

  useEffect(() => {
    hubConnection.start()
      .catch(error => console.error('Error connecting to SignalR: ', error));

    return () => {
      hubConnection.stop();
    };
  }, [hubConnection]);

  return (
    <SignalRContext.Provider value={hubConnection}>
      {children}
    </SignalRContext.Provider>
  );
};
```
	
3. Use the SignalR Context: In your component, use the SignalR context to handle events.

```
import { useEffectimport { useEffect } from 'react';
import { useSignalR } from './signalr-context';

const SomeComponent = () => {
  const hubConnection = useSignalR();

  useEffect(() => {
    const handleFineTuneCreated = (fineTuning) => {
      // Handle FineTuneCreated event
    };

    hubConnection.on('FineTuneCreated', handleFineTuneCreated);

    // Add other event listeners as needed

    // Cleanup event listeners on unmount
    return () => {
      hubConnection.off('FineTuneCreated', handleFineTuneCreated);
      // Remove other event listeners as needed
    };
  }, [hubConnection]);

  // Component rendering
};
```

# MasterAdmin Guide for Using Linear Regression

### Introduction:
As a MasterAdmin, you can leverage the power of machine learning to provide valuable insights and predictions for your clients. One of the most widely used techniques in machine learning is Linear Regression. This guide will help you understand what Linear Regression is, how to train a model, and how to structure your Order data for training.

### What is Linear Regression?
Linear Regression is a supervised machine learning algorithm that models the relationship between one or more input features (independent variables) and a continuous output (dependent variable). The algorithm tries to find the best-fitting straight line (or hyperplane in the case of multiple input features) that minimizes the difference between the predicted and actual output values.

### Training a Linear Regression Model:
Training a Linear Regression model involves the following steps:

Collect historical data: Gather historical data on orders with relevant input features and actual output values. The input features could be order size, distance, shipping method, or any other factors that influence the output value (e.g., delivery time). The actual output values are the observed delivery times for these orders.

Preprocess the data: Clean the data by removing any outliers or erroneous values. Normalize or scale the data if necessary, so that all input features have a similar range of values.

Split the data: Divide the dataset into two parts: a training set (typically 70-80% of the data) and a testing set (the remaining 20-30%). The training set is used to train the model, while the testing set is used to evaluate its performance.

Train the model: Use the training set to train the Linear Regression model. The model will learn the relationship between the input features and the output values by adjusting its internal parameters (weights and intercept) to minimize the difference between the predicted and actual output values.

Evaluate the model: Use the testing set to assess the model's performance. Calculate metrics such as Mean Absolute Error (MAE), Mean Squared Error (MSE), or R-squared to determine how well the model generalizes to new, unseen data.

### Structuring Order Data for Training:
To train a Linear Regression model with order data, you need to organize the data into a structured format. The data should consist of a matrix of input features (X) and a vector of output values (y).

Here's an example of how to structure order data:
```
Order Size	Distance	Shipping Method	Delivery Time
10	5	1	2.5
20	10	2	4
30	15	1	5.5
...	...	...	...
```

Order Size: The number of items in the order.
Distance: The distance between the warehouse and the delivery location.
Shipping Method: A numeric representation of the shipping method (e.g., 1 for standard shipping, 2 for express shipping).
Delivery Time: The actual delivery time for the order (in days).

In this example, the input features matrix (X) would consist of the columns "Order Size," "Distance," and "Shipping Method," while the output values vector (y) would be the "Delivery Time" column.

Once you have structured your order data, you can use it to train and evaluate the Linear Regression model as described earlier in this guide.

Remember that training a machine learning model requires a good understanding of the problem domain, data preprocessing, and model evaluation. Always experiment with different input features, model parameters, and evaluation metrics to find the best-fitting model

## Choosing the right algorithm

Choosing the right algorithm for your machine learning problem can be challenging, especially if you're new to the field. Here are some guidelines to help you make an informed decision:

Understand the problem: Begin by understanding the problem you're trying to solve. Determine if it's a classification problem (predicting discrete labels), a regression problem (predicting continuous values), or an unsupervised learning problem (discovering patterns in data).

Analyze the data: Examine the data and its features. Consider the size of the dataset, the type of features (numeric, categorical, etc.), and the relationships between features and the target variable. Also, check for any missing or noisy data that might require preprocessing.

Consider model complexity: Different algorithms have varying levels of complexity. Simple models (e.g., linear regression) might work well with small datasets and linear relationships, while more complex models (e.g., neural networks) can capture non-linear patterns but may require more data and computational resources.

Evaluate performance: Use evaluation metrics (such as accuracy, precision, recall, F1-score for classification problems, and Mean Absolute Error, Mean Squared Error, R-squared for regression problems) to assess the performance of different algorithms on your dataset. Keep in mind that a model that performs well on the training set might not necessarily generalize well to new, unseen data, so using a testing set or cross-validation is essential.

Consider interpretability: Some algorithms, like decision trees and linear regression, are easily interpretable, which can be beneficial in understanding the underlying relationships between features and the target variable. On the other hand, more complex models like deep neural networks can be challenging to interpret, making it harder to explain their predictions.

In the given example, since the goal is to predict delivery times (a continuous variable), a regression algorithm is appropriate. Linear regression is a simple and interpretable algorithm that assumes a linear relationship between input features and the target variable. This choice was based on the assumption that the relationship between features (e.g., order size, distance, shipping method) and delivery times might be linear. However, depending on the complexity of the problem and the dataset, other regression algorithms like support vector regression or decision tree regression might be more suitable.

Always experiment with multiple algorithms and evaluate their performance on your specific problem and dataset. This iterative process can help you find the most suitable algorithm for your needs.
