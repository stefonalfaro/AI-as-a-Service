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
