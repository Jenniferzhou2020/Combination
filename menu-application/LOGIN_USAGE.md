# Login Component Usage Guide

## Overview

The login component provides a secure authentication interface with email/password validation, token management, and automatic redirection.

## Features

- Email validation with regex pattern
- Password validation (minimum 8 characters)
- Show/hide password toggle
- Remember me checkbox
- Loading state with spinner
- Error message display
- Automatic redirection after login
- Return URL support
- Responsive design
- Internationalization support (English/French)

## Accessing the Login Page

Navigate to: `http://localhost:4200/login`

## Form Validation

### Email Field
- Required field
- Must be a valid email format
- Pattern: `[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}`

### Password Field
- Required field
- Minimum 8 characters
- Can toggle visibility

## Using the Auth Guard

Protect routes that require authentication:

```typescript
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  {
    path: 'protected',
    component: ProtectedComponent,
    canActivate: [authGuard]  // Add this line
  }
];
```

When a user tries to access a protected route without being authenticated:
1. They are redirected to `/login`
2. The original URL is saved as `returnUrl` query parameter
3. After successful login, they are redirected back to the original URL

## Programmatic Login

```typescript
import { AuthFacade } from './facades/auth.facade';
import { LoginRequest } from './models/auth.models';

constructor(private authFacade: AuthFacade) {}

login() {
  const credentials: LoginRequest = {
    email: 'user@example.com',
    password: 'password123'
  };

  this.authFacade.login(credentials).subscribe({
    next: (response) => {
      console.log('Token:', response.token);
      console.log('User:', response.user);
      // Navigate or perform actions
    },
    error: (error) => {
      console.error('Login failed:', error.message);
    }
  });
}
```

## Checking Authentication Status

```typescript
// In component
constructor(private authFacade: AuthFacade) {}

ngOnInit() {
  // Check if authenticated
  if (this.authFacade.isAuthenticated()) {
    console.log('User is logged in');
  }

  // Subscribe to auth status changes
  this.authFacade.authStatus$.subscribe(status => {
    console.log('Authenticated:', status.isAuthenticated);
    console.log('User:', status.user);
    console.log('Token:', status.token);
  });
}
```

## Logout

```typescript
logout() {
  this.authFacade.logout().subscribe({
    next: () => {
      console.log('Logged out successfully');
      this.router.navigate(['/login']);
    },
    error: (error) => {
      console.error('Logout error:', error);
    }
  });
}
```

## Getting Current User

```typescript
const user = this.authFacade.getUser();
if (user) {
  console.log('User ID:', user.id);
  console.log('Email:', user.email);
  console.log('Name:', user.name);
  console.log('Role:', user.role);
}
```

## Getting Token

```typescript
const token = this.authFacade.getToken();
if (token) {
  console.log('Access token:', token);
}
```

## API Integration

The login component expects your backend API to:

### Login Endpoint
- **URL**: `/api/auth/login`
- **Method**: POST
- **Request Body**:
```json
{
  "email": "user@example.com",
  "password": "password123"
}
```

- **Response**:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "refresh_token_here",
  "user": {
    "id": "123",
    "email": "user@example.com",
    "name": "John Doe",
    "role": "user"
  },
  "expiresIn": 3600
}
```

### Refresh Token Endpoint
- **URL**: `/api/auth/refresh`
- **Method**: POST
- **Request Body**:
```json
{
  "refreshToken": "refresh_token_here"
}
```

- **Response**:
```json
{
  "token": "new_access_token",
  "refreshToken": "new_refresh_token",
  "expiresIn": 3600
}
```

### Logout Endpoint
- **URL**: `/api/auth/logout`
- **Method**: POST
- **Headers**: `Authorization: Bearer {token}`

## Automatic Token Refresh

The system automatically refreshes the access token:
- 5 minutes before expiration
- On 401 Unauthorized responses
- Logs are created for all refresh attempts

## Error Handling

All errors are:
1. Logged to the logging system
2. Translated to user's language
3. Displayed in the UI
4. Available in the log viewer

## Styling Customization

Modify `login.component.css` to customize:
- Colors and gradients
- Card dimensions
- Button styles
- Error message appearance
- Responsive breakpoints

## Translation Keys

Add these keys to your translation files:

```json
{
  "auth": {
    "login": "Login",
    "logout": "Logout",
    "email": "Email",
    "password": "Password",
    "remember_me": "Remember me",
    "forgot_password": "Forgot password?",
    "login_success": "Login successful",
    "logout_success": "Logout successful"
  },
  "validation": {
    "required": "This field is required",
    "email_invalid": "Please enter a valid email address",
    "password_min_length": "Password must be at least 8 characters"
  }
}
```

## Security Best Practices

1. **Never log passwords** - The logger automatically excludes password fields
2. **Use HTTPS** - Always use secure connections in production
3. **Token Storage** - Tokens are stored in localStorage (consider httpOnly cookies for production)
4. **Token Expiration** - Tokens automatically refresh before expiration
5. **CSRF Protection** - Implement CSRF tokens for state-changing operations

## Testing

### Manual Testing
1. Navigate to `/login`
2. Try invalid email formats
3. Try passwords less than 8 characters
4. Test successful login
5. Verify token is stored
6. Test logout
7. Test protected route access

### Example Test Data
```typescript
// Valid credentials
email: 'test@example.com'
password: 'password123'

// Invalid email
email: 'invalid-email'
password: 'password123'

// Short password
email: 'test@example.com'
password: 'short'
```

## Troubleshooting

### Login button not working
- Check browser console for errors
- Verify API endpoint is correct
- Check network tab for request/response

### Token not persisting
- Check localStorage in browser DevTools
- Verify browser allows localStorage
- Check for private/incognito mode

### Redirect not working
- Check router configuration
- Verify returnUrl parameter
- Check auth guard implementation

### Validation not showing
- Verify form is touched
- Check translation keys exist
- Verify Material modules are imported
