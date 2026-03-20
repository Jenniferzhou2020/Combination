# Application Logging System

This Angular application includes a comprehensive logging system that captures exceptions, HTTP requests, and application events.

## Features

- **Multiple Log Levels**: DEBUG, INFO, WARN, ERROR, FATAL
- **Automatic Error Capture**: Unhandled errors and promise rejections
- **HTTP Request Logging**: All HTTP requests and responses
- **Local Storage**: Logs persisted in browser localStorage
- **Export Functionality**: Download logs as .log or .json files
- **Log Viewer UI**: Visual interface to view and filter logs

## Log Levels

- **DEBUG**: Detailed information for debugging
- **INFO**: General informational messages
- **WARN**: Warning messages for potentially harmful situations
- **ERROR**: Error events that might still allow the application to continue
- **FATAL**: Severe error events that might cause the application to abort

## Usage

### In Components/Services

```typescript
import { LoggerService } from './services/logger.service';

constructor(private logger: LoggerService) {}

// Log different levels
this.logger.debug('Debug message', { data: 'value' });
this.logger.info('Info message', { userId: 123 });
this.logger.warn('Warning message', { reason: 'something' });
this.logger.error('Error occurred', { error: errorObj }, error);
this.logger.fatal('Fatal error', { critical: true }, error);
```

### Viewing Logs

Add the log viewer component to your route:

```typescript
import { LogViewerComponent } from './components/log-viewer/log-viewer.component';

// In your routes
{
  path: 'logs',
  component: LogViewerComponent
}
```

### Exporting Logs

```typescript
// Export as text file
this.logger.exportLogsAsFile('my-logs.log');

// Export as JSON
this.logger.exportLogsAsJson('my-logs.json');
```

### Configuration

```typescript
this.logger.setConfig({
  enableConsole: true,      // Log to browser console
  enableStorage: true,      // Save to localStorage
  maxLogEntries: 1000,      // Maximum logs to keep
  logLevel: LogLevel.DEBUG  // Minimum level to log
});
```

## Automatic Logging

The following are logged automatically:

1. **HTTP Requests**: All requests with method, URL, headers, body
2. **HTTP Responses**: Status, duration, response body
3. **HTTP Errors**: Error details with status codes
4. **Authentication Events**: Login, logout, token refresh
5. **Unhandled Errors**: Global error handler captures all unhandled errors
6. **Promise Rejections**: Unhandled promise rejections

## Log Storage

- Logs are stored in browser localStorage under the key `app_logs`
- Maximum 5000 log entries are stored
- Older logs are automatically removed when limit is reached
- Logs persist across browser sessions

## Interceptors

Three interceptors work together:

1. **Logging Interceptor**: Logs all HTTP requests/responses
2. **Error Interceptor**: Logs HTTP errors with translations
3. **Auth Interceptor**: Handles authentication (also logged)

## Log File Format

### Text Format (.log)
```
[2026-03-06T10:30:45.123Z] [INFO] Login attempt
Data: {"email":"user@example.com"}
URL: http://localhost:4200/login
--------------------------------------------------------------------------------
```

### JSON Format (.json)
```json
[
  {
    "timestamp": "2026-03-06T10:30:45.123Z",
    "level": "INFO",
    "message": "Login attempt",
    "data": {"email":"user@example.com"},
    "url": "http://localhost:4200/login",
    "userAgent": "Mozilla/5.0..."
  }
]
```

## Best Practices

1. Use appropriate log levels
2. Include relevant context data
3. Don't log sensitive information (passwords, tokens)
4. Export logs before clearing for long-term storage
5. Review ERROR and FATAL logs regularly
6. Use DEBUG level during development only

## Privacy & Security

- Logs may contain user data - handle with care
- Don't expose logs in production builds
- Sanitize sensitive data before logging
- Consider implementing log rotation
- Export and store logs securely

## Troubleshooting

### Logs not appearing
- Check log level configuration
- Verify localStorage is not full
- Check browser console for errors

### Export not working
- Ensure browser allows downloads
- Check popup blocker settings
- Verify sufficient disk space

### Performance issues
- Reduce maxLogEntries
- Increase log level (e.g., WARN or ERROR only)
- Disable console logging in production
