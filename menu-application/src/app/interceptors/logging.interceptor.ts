import { HttpInterceptorFn, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { tap, catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { LoggerService } from '../services/logger.service';

function safeClone(obj: any, maxDepth: number = 3, currentDepth: number = 0): any {
  if (obj === null || obj === undefined) {
    return obj;
  }

  // Handle primitive types
  if (typeof obj !== 'object' && typeof obj !== 'function') {
    return obj;
  }

  // Don't clone functions
  if (typeof obj === 'function') {
    return '[Function]';
  }

  // Check depth limit
  if (currentDepth >= maxDepth) {
    return '[Max depth reached]';
  }
  
  try {
    // For large objects, provide a summary instead of full content
    if (typeof obj === 'object' && obj !== null) {
      const keys = Object.keys(obj);
      if (keys.length > 10) {
        return `[Object with ${keys.length} properties: ${keys.slice(0, 3).join(', ')}...]`;
      }
    }

    // Use structured clone if available (safer than JSON)
    if (typeof structuredClone !== 'undefined') {
      return structuredClone(obj);
    }
    
    // Fallback to JSON serialization
    return JSON.parse(JSON.stringify(obj));
  } catch (error) {
    // Handle circular references, functions, or non-serializable objects
    try {
      // Try to extract basic info
      if (obj.constructor && obj.constructor.name) {
        return `[${obj.constructor.name}]`;
      }
      return '[Non-serializable object]';
    } catch (e) {
      return String(obj);
    }
  }
}

function shouldLogResponseBody(url: string, body: any): boolean {
  // Don't log large translation files or static assets
  if (url.includes('/assets/i18n/') || url.includes('.json')) {
    return false;
  }
  
  // Don't log very large response bodies
  if (body && typeof body === 'object') {
    const bodyStr = JSON.stringify(body);
    if (bodyStr.length > 1000) {
      return false;
    }
  }
  
  return true;
}

function shouldLogRequestBody(url: string, body: any): any {
  // Don't log sensitive authentication data
  if (url.includes('/auth/login') || url.includes('/auth/refresh')) {
    return '[Authentication data omitted for security]';
  }
  
  return safeClone(body);
}

export const loggingInterceptor: HttpInterceptorFn = (req, next) => {
  const logger = inject(LoggerService);
  const startTime = Date.now();

  // Skip logging for static assets if desired
  const isStaticAsset = req.url.includes('/assets/') || req.url.endsWith('.json') || req.url.endsWith('.css') || req.url.endsWith('.js');
  
  // Log request (use DEBUG level for static assets, INFO for API calls)
  const logLevel = isStaticAsset ? 'debug' : 'info';
  
  if (logLevel === 'info' || !isStaticAsset) {
    logger.debug('HTTP Request', {
      method: req.method,
      url: req.url,
      headers: req.headers.keys().reduce((acc, key) => {
        // Don't log sensitive headers
        if (key.toLowerCase() === 'authorization') {
          acc[key] = '[REDACTED]';
        } else {
          acc[key] = req.headers.get(key);
        }
        return acc;
      }, {} as any),
      body: shouldLogRequestBody(req.url, req.body)
    });
  }

  return next(req).pipe(
    tap(event => {
      if (event instanceof HttpResponse) {
        const duration = Date.now() - startTime;
        
        // Only log successful responses for static assets at debug level
        if (isStaticAsset && event.status === 200) {
          logger.debug('HTTP Response', {
            method: req.method,
            url: req.url,
            status: event.status,
            duration: `${duration}ms`,
            body: '[Static asset loaded successfully]'
          });
        } else {
          logger.info('HTTP Response', {
            method: req.method,
            url: req.url,
            status: event.status,
            statusText: event.statusText,
            duration: `${duration}ms`,
            body: shouldLogResponseBody(req.url, event.body) ? safeClone(event.body) : '[Response body omitted - large or static content]'
          });
        }
      }
    }),
    catchError((error: HttpErrorResponse) => {
      const duration = Date.now() - startTime;
      
      logger.error('HTTP Error', {
        method: req.method,
        url: req.url,
        status: error.status,
        statusText: error.statusText,
        duration: `${duration}ms`,
        error: safeClone(error.error),
        message: error.message
      });
      
      return throwError(() => error);
    })
  );
};
